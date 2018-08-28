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

        //Constructor
        public PakizhTarasAlgorithm()
        {
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

        //Algorithm
        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            Robot.Common.Robot movingRobot = robots[robotToMoveIndex];
            if ((movingRobot.Energy > 500) && (robots.Count < map.Stations.Count))
            {
                return new CreateNewRobotCommand();
            }
            Position stationPosition = FindNearestFreeStation(robots[robotToMoveIndex], map, robots);
            if (stationPosition == null)
                return null;
            if (stationPosition == movingRobot.Position)
                return new CollectEnergyCommand();
            else
            {
                return new MoveCommand() { NewPosition = stationPosition };
            }
        }

        public Position FindNearestFreeStation(Robot.Common.Robot movingRobot, Map map, IList<Robot.Common.Robot> robots)
        {
            EnergyStation nearest = null;
            int minDistance = int.MaxValue;
            foreach (var station in map.Stations)
            {
                if (IsStationFree(station, movingRobot, robots))
                {
                    int d = Helper.FindDistance(station.Position, movingRobot.Position);
                    if (d < minDistance)
                    {
                        minDistance = d;
                        nearest = station;

                    }
                }
            }
            return nearest == null ? null : nearest.Position;
        }
        public bool IsStationFree(EnergyStation station, Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
        {
            return IsCellFree(station.Position, movingRobot, robots);
        }
        public bool IsCellFree(Position cell, Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
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
    }
}
