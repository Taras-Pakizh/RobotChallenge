using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

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

        //Constructor
        public PakizhTarasAlgorithm()
        {
            Round = 0;
            Logger.OnLogRound += (sender, e) => Round++;
        }
        public PakizhTarasAlgorithm(IList<Robot.Common.Robot> _robots, int _robotToMoveIndex, Map _map)
        {
            Init(_robots, _robotToMoveIndex, _map);
            Round = 0;
            Logger.OnLogRound += (sender, e) => Round++;
        }

        #region Interface_Description
        public string Author
        {
            get { return "Pakizh Taras PZ-34"; }
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
            
            if((movingRobot.Energy > Helper.EnergyToBorn) && (myRobotsCount <= 100) && 
                (myRobotsCount <= map.Stations.Count) && (Round < Helper.RoundToStop))
                return new CreateNewRobotCommand(){ NewRobotEnergy = 200 };
                
                
            if (IsCollecting(movingRobot.Position) && IsStationFree(sortedStations[0]))
                return new CollectEnergyCommand();
                
                
            EnergyStation station = FindNearestFreeStation();
            do
            {
                if (station == null)
                    station = FindNearestOccupiedStation();
                Position position = FindNearestFreeCellAroundStation(station);
                position = GetNextPosition(position, out int steps);
                if(steps > 5 && IsStationFree(station))
                {
                    station = null;
                    continue;
                }
                if(position == movingRobot.Position) 
                    return new CollectEnergyCommand();
                return new MoveCommand() { NewPosition = GetNextPosition(position) };
            }while(true);
        }

        //StrategyOfMoving
        public Position GetNextPosition(Position position, out int steps)
        {
            Position result = position.Copy();
            steps = 1;
            while (true)
            {
                int energySpend = Helper.FindDistance(result, movingRobot.Position) * index;
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
                    if (positions[row][col] != null || !IsCellFree(positions[row][col]))
                        continue;
                    positions[row][col] = new Position(positions[0][0].X + col, positions[0][0].Y);
                    if (!IsCellValid(positions[row][col])) continue;
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
                if (robot != movingRobot && Helper.CanCollect(station, robot.Position))
                    return false;
            return true;
        }
        public bool IsCollecting(Position position)
        {
            foreach (var station in sortedStations)
                if (Helper.CanCollect(station, position))
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

        //Init
        public void Init(IList<Robot.Common.Robot> _robots, int _robotToMoveIndex, Map _map)
        {
            robots = _robots;
            map = _map;
            movingRobot = robots[_robotToMoveIndex];
            //map.Stations.CopyTo(sortedStations, 0);
            sortedStations = map.Stations.ToArray();
            sortedStations = sortedStations.OrderBy(x => Helper.FindDistance(x.Position, movingRobot.Position)).ToArray();
            myRobotsCount = robots.Count(x => x.Owner == movingRobot.Owner);
        }
    }
}
