using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;
using System.IO;

namespace Pakizh.Taras.RobotChallenge
{
    public class PakizhTarasAlgorithm : IRobotAlgorithm
    {
        #region Variables
        public int Round { get; set; }
        public Robot.Common.Robot movingRobot;
        public Map map;
        public IList<Robot.Common.Robot> robots;
        public EnergyStation[] sortedStations;
        public int myRobotsCount;
        public Dictionary<int, Position> PropertyBook;
        public Dictionary<int, Position> TargetBook;
        public int MyRobotId;
        #endregion

        //Events
        public event SubWindow.RobotLogicHaldler RobotLogic;

        //Constructor
        public PakizhTarasAlgorithm()
        {
            Round = 0;
            Logger.OnLogRound += (sender, e) => Round++;

            SubWindow.MainWindow CommonMassegeWindow = new SubWindow.MainWindow();
            Logger.OnLogMessage += CommonMassegeWindow.AddMessage;
            CommonMassegeWindow.Show();
            CommonMassegeWindow.StartLogging += (sender, e) => Logger.OnLogMessage += CommonMassegeWindow.AddMessage;
            CommonMassegeWindow.StopLogging += (sender, e) => Logger.OnLogMessage -= CommonMassegeWindow.AddMessage;

            SubWindow.MainWindow RobotLogicMassegeWindow = new SubWindow.MainWindow();
            RobotLogic += RobotLogicMassegeWindow.AddRobotMessage;
            RobotLogicMassegeWindow.Show();
            RobotLogicMassegeWindow.StartLogging += (sender, e) => RobotLogic += RobotLogicMassegeWindow.AddRobotMessage;
            RobotLogicMassegeWindow.StopLogging += (sender, e) => RobotLogic -= RobotLogicMassegeWindow.AddRobotMessage;

            PropertyBook = new Dictionary<int, Position>();
            TargetBook = new Dictionary<int, Position>();
        }

        //Init
        public void Init(IList<Robot.Common.Robot> _robots, int _robotToMoveIndex, Map _map)
        {
            robots = _robots;
            map = _map;
            movingRobot = robots[_robotToMoveIndex];
            MyRobotId = _robotToMoveIndex;
            myRobotsCount = robots.Count(x => x.Owner == movingRobot.Owner);

            sortedStations = map.Stations.Where(x=>!PropertyBook.ContainsValue(x.Position)).ToArray(); 
            sortedStations = sortedStations.Where(x => TargetBook.Count(y => y.Value == x.Position) <= Helper.maxStationTarget).ToArray();
            sortedStations = sortedStations.OrderBy(x => Helper.FindDistance(x.Position, movingRobot.Position)).ToArray();
        }

        #region Interface_Description
        public string Author
        {
            get { return "Pakizh Taras"; }
        }
        public string Description
        {
            get { return "I tried"; }
        }
        #endregion

        public RobotCommand DoStep(IList<Robot.Common.Robot> _robots, int _robotToMoveIndex, Map _map)
        {
            RobotCommand command = null;
            Init(_robots, _robotToMoveIndex, _map);
            command = GetCreateNewRobotCommand();
            if (command == null)
                command = GetCollectEnergyCommand();
            if (command == null)
            {
                command = GetMoveCommand();
                if (((MoveCommand)command).NewPosition == movingRobot.Position)
                    command = new CollectEnergyCommand();
            }
            return command;
        }

        //Commands
        public CreateNewRobotCommand GetCreateNewRobotCommand()
        {
            CreateNewRobotCommand command = null;
            if ((myRobotsCount < 100) && (myRobotsCount < map.Stations.Count) && (Round < Helper.RoundToStop))
            {
                int distance = Helper.FindDistance(movingRobot.Position, sortedStations[0].Position);
                if (distance < 100 && movingRobot.Energy > (100 + Helper.EnergyToBorn))
                    command = new CreateNewRobotCommand() { NewRobotEnergy = 100 };
                else if (distance < 300 && movingRobot.Energy > (300 + Helper.EnergyToBorn))
                    command = new CreateNewRobotCommand() { NewRobotEnergy = 300 };
                else if (distance < 2000 && movingRobot.Energy > (1000 + Helper.EnergyToBorn))
                    command = new CreateNewRobotCommand() { NewRobotEnergy = 1000 };
            }
            return command;
        }
        public CollectEnergyCommand GetCollectEnergyCommand()
        {
            if (PropertyBook.ContainsKey(MyRobotId))
            {
                if (Helper.CanCollect(PropertyBook[MyRobotId], movingRobot.Position))
                    return new CollectEnergyCommand();
                else return null;
            }
            if (Helper.IsCollecting(movingRobot.Position, sortedStations))
            {
                var stations = GetCollectedStations();
                PropertyBook.Add(MyRobotId, stations.OrderByDescending(x=>x.Energy).First().Position);
                RobotLogic?.Invoke(this, new SubWindow.RobotLogicEventArgs()
                {
                    station = PropertyBook[MyRobotId],
                    RobotId = MyRobotId,
                    robot = robots[MyRobotId],
                    robotEvent = SubWindow.RobotLogicEventArgs.RobotEvents.PropertyAdded
                });
                if (TargetBook.ContainsKey(MyRobotId))
                {
                    RobotLogic?.Invoke(this, new SubWindow.RobotLogicEventArgs()
                    {
                        station = TargetBook[MyRobotId],
                        RobotId = MyRobotId,
                        robot = robots[MyRobotId],
                        robotEvent = SubWindow.RobotLogicEventArgs.RobotEvents.TargetAdded
                    });
                    TargetBook.Remove(MyRobotId);
                }
                while (TargetBook.ContainsValue(PropertyBook[MyRobotId]))
                {
                    RobotLogic?.Invoke(this, new SubWindow.RobotLogicEventArgs()
                    {
                        station = TargetBook[MyRobotId],
                        RobotId = MyRobotId,
                        robot = robots[MyRobotId],
                        robotEvent = SubWindow.RobotLogicEventArgs.RobotEvents.TargetRemoved
                    });
                    TargetBook.Remove(TargetBook.Where(x => x.Value == PropertyBook[MyRobotId]).First().Key);
                }
                return new CollectEnergyCommand();
            }
            return null;
        }
        public MoveCommand GetMoveCommand()
        {
            EnergyStation station = null;
            if (PropertyBook.ContainsKey(MyRobotId))
            {
                station = map.Stations.First(x => x.Position == PropertyBook[MyRobotId]);
            }
            else if (TargetBook.ContainsKey(MyRobotId))
            {
                station = map.Stations.First(x => x.Position == TargetBook[MyRobotId]);
            }

            bool checker = true;
            if (station == null)
                station = FindNearestFreeStation();
            do
            {
                if (station == null)
                    station = FindNearestOccupiedStation();
                if (station == null && !checker)
                    station = FindNearestFreeStation();
                Position position = FindNearestFreeCellAroundStation(station);
                RobotMoving strategyOfMoving = new RobotMoving(movingRobot, robots);
                position = strategyOfMoving.GetNextPosition(position, out int steps);
                if (checker && steps > 5 && Helper.IsStationFree(station, robots, movingRobot))
                {
                    station = null;
                    checker = false;
                    continue;
                }
                if (!TargetBook.ContainsKey(MyRobotId) && !PropertyBook.ContainsKey(MyRobotId))
                {
                    TargetBook.Add(MyRobotId, station.Position);
                    RobotLogic?.Invoke(this, new SubWindow.RobotLogicEventArgs()
                    {
                        station = TargetBook[MyRobotId],
                        RobotId = MyRobotId,
                        robot = robots[MyRobotId],
                        robotEvent = SubWindow.RobotLogicEventArgs.RobotEvents.TargetAdded
                    });
                }
                return new MoveCommand() { NewPosition = position };
            } while (true);
        }

        //Find Station
        public EnergyStation FindNearestFreeStation()
        {
            foreach (var station in sortedStations)
                if (Helper.IsStationFree(station, robots, movingRobot))
                    return station;
            return null;
        }
        public EnergyStation FindNearestOccupiedStation()
        {
            foreach(var station in sortedStations)
            {
                if (!Helper.IsStationFree(station, robots, movingRobot))
                    return station;
            }
            return null;
        }
        public List<EnergyStation> GetCollectedStations()
        {
            List<EnergyStation> stations = new List<EnergyStation>();
            foreach(var station in sortedStations)
                if (Helper.CanCollect(station.Position, movingRobot.Position))
                    stations.Add(station);
            return stations;
        }

        //Find Cell
        public Position FindNearestFreeCellAroundStation(EnergyStation station)
        {
            Position[][] positions = new Position[Helper.distance * 2 + 1][];
            for (int i = 0; i < positions.Length; ++i)
                positions[i] = new Position[positions.Length];
            positions[0][0] = new Position(station.Position.X - Helper.distance, station.Position.Y - Helper.distance);

            int minDistance = int.MaxValue;
            Position result = null;
            for(int row = 0, distance; row < positions.Length; ++row)
            {
                if (positions[row][0] == null)
                    positions[row][0] = new Position(positions[0][0].X, positions[0][0].Y + row);
                for(int col = 0; col < positions.Length; ++col)
                {
                    if(positions[row][col] == null)
                        positions[row][col] = new Position(positions[0][0].X + col, positions[row][0].Y);
                    if (!Helper.IsCellValid(positions[row][col]) || !Helper.IsCellFree(positions[row][col], robots, movingRobot))
                        continue;
                    distance = Helper.FindDistance(movingRobot.Position, positions[row][col]);
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        result = positions[row][col];
                    }
                }
            }
            return result;
        }
    }
}
