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
        //Vars
        private int Round { get; set; }
        private Robot.Common.Robot movingRobot;
        private Map map;
        private IList<Robot.Common.Robot> robots;
        private EnergyStation[] sortedStations;
        private int myRobotsCount;
        private Dictionary<int, Position> PropertyBook;
        private int MyRobotId;

        //Constructor
        public PakizhTarasAlgorithm()
        {
            Round = 0;
            Logger.OnLogRound += (sender, e) => Round++;
            PropertyBook = new Dictionary<int, Position>();
        }

        //Init
        public void Init(IList<Robot.Common.Robot> _robots, int _robotToMoveIndex, Map _map)
        {
            robots = _robots;
            map = _map;
            movingRobot = robots[_robotToMoveIndex];
            MyRobotId = _robotToMoveIndex;
            sortedStations = map.Stations.Where(x=>!PropertyBook.ContainsValue(x.Position)).ToArray();
            sortedStations = sortedStations.OrderBy(x => Helper.FindDistance(x.Position, movingRobot.Position)).ToArray();
            myRobotsCount = robots.Count(x => x.Owner == movingRobot.Owner);
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

        /// <summary>
        /// First algorithm
        /// </summary>
        /// <param name="robots">All robots on the map</param>
        /// <param name="robotToMoveIndex">my Robot</param>
        /// <param name="map">Map</param>
        /// <returns>Command</returns>
        public RobotCommand DoStep(IList<Robot.Common.Robot> _robots, int _robotToMoveIndex, Map _map)
        {
            Init(_robots, _robotToMoveIndex, _map);

            RobotCommand command = null;
            command = GetCreateNewRobotCommand();
            if (command == null)
                command = GetCollectEnergyCommand();
            if (command == null)
                command = GetMoveCommand();
            using(StreamWriter sw = new StreamWriter(@"log.txt", true, Encoding.Default))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Id = " + MyRobotId.ToString() + " " + movingRobot.Position + " ");
                builder.Append(command.ToString() + " ");
                if(command is MoveCommand)
                {
                    builder.Append(((MoveCommand)command).NewPosition);
                }
                sw.WriteLine(builder);
            }
            return command;
        }

        //Commands
        public CreateNewRobotCommand GetCreateNewRobotCommand()
        {
            CreateNewRobotCommand command = null;
            if ((myRobotsCount <= 100) && (myRobotsCount < map.Stations.Count) && (Round < Helper.RoundToStop))
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
            if (IsCollecting(movingRobot.Position))
            {
                var stations = GetCollectedStations();
                PropertyBook.Add(MyRobotId, stations.OrderByDescending(x=>x.Energy).First().Position);
                return new CollectEnergyCommand();
            }
            return null;
        }
        public MoveCommand GetMoveCommand()
        {
            EnergyStation station = null;
            if (PropertyBook.ContainsKey(MyRobotId))
            {
                if(IsStationFree(new EnergyStation() { Position = PropertyBook[MyRobotId] }))
                    station = map.Stations.First(x => x.Position == PropertyBook[MyRobotId]);
                else
                {
                    int distance = Helper.FindDistance(movingRobot.Position, sortedStations[0].Position);
                    if(distance < movingRobot.Energy && distance < 300)
                    {
                        PropertyBook[MyRobotId] = sortedStations[0].Position;
                        station = sortedStations[0];
                    }
                    else station = station = map.Stations.First(x => x.Position == PropertyBook[MyRobotId]);
                }
            }

            if(station == null)
                station = FindNearestFreeStation();
            do
            {
                if (station == null)
                    station = FindNearestOccupiedStation();
                Position position = FindNearestFreeCellAroundStation(station);
                position = GetNextPosition(position, out int steps);
                if (steps > 5 && IsStationFree(station))
                {
                    station = null;
                    continue;
                }
                return new MoveCommand() { NewPosition = position };
            } while (true);
        }

        //StrategyOfMoving
        public Position GetNextPosition(Position position, out int steps)
        {
            Position result = position.Copy();
            steps = 1;
            while (true)
            {
                int energySpend = Helper.FindDistance(result, movingRobot.Position) * steps;
                if(movingRobot.Energy < energySpend)
                {
                    result = DivideWayBy2(result);
                    steps *= 2;
                    continue;
                }
                break;
            }
            return result;
        }
        public Position DivideWayBy2(Position position)
        {
            int length = Math.Abs(movingRobot.Position.X - position.X) + Math.Abs(movingRobot.Position.Y - position.Y);
            int way = length / 2;
            while(length > way)
            {
                if(Math.Abs(position.X - movingRobot.Position.X) > Math.Abs(position.Y - movingRobot.Position.Y))
                {
                    if (position.X > movingRobot.Position.X)
                        position.X -= 1;
                    else position.X += 1;
                }
                else
                {
                    if (position.Y > movingRobot.Position.Y)
                        position.Y -= 1;
                    else position.Y += 1;
                }
                if ((length - way == 1))
                {
                    if (IsCellFree(position))
                        length--;
                    else continue;
                }
                else
                {
                    length--;
                }
            }
            return position;
        }

        //Find Station
        public EnergyStation FindNearestFreeStation()
        {
            foreach (var station in sortedStations)
                if (IsStationFree(station))
                    return station;
            return null;
        }
        public EnergyStation FindNearestOccupiedStation()
        {
            foreach(var station in sortedStations)
            {
                if (IsStationFree(station)) continue;
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
                        positions[row][col] = new Position(positions[0][0].X + col, positions[0][0].Y);
                    if (!IsCellValid(positions[row][col]) || !IsCellFree(positions[row][col]))
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

        //Is true
        public bool IsCellFree(Position cell)
        {
            foreach (var robot in robots)
            {
                if (robot != movingRobot)
                {
                    if (robot.Position == cell)
                        return false;
                }
            }
            return true;
        }
        public bool IsStationFree(EnergyStation station)
        {
            foreach (var robot in robots)
                if (robot != movingRobot && Helper.CanCollect(station.Position, robot.Position))
                    return false;
            return true;
        }
        public bool IsCollecting(Position position)
        {
            foreach (var station in sortedStations)
                if (Helper.CanCollect(station.Position, position))
                    return true;
            return false;
        }
        public bool IsCellValid(Position position)
        {
            if (position.X > -1 && position.X < 100)
                if (position.Y > -1 && position.Y < 100)
                    return true;
            return false;
        }
    }
}
