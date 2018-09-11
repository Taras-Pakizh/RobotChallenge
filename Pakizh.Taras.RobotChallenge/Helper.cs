using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace Pakizh.Taras.RobotChallenge
{
    public static class Helper
    {
        public static int distance = 3;
        public static int EnergyToBorn = 50;
        public static int RoundToStop = 40;
        public static int maxRestoredEnergy = 200;
        public static int maxStationTarget = 5;

        public static int FindDistance(Position a, Position b)
        {
            int distanceX = Math.Abs(a.X - b.X);
            int distanceY = Math.Abs(a.Y - b.Y);
            if (distanceX > 50) distanceX = 100 - distanceX;
            if (distanceY > 50) distanceY = 100 - distanceY;
            return (int)(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));
        }
        public static bool CanCollect(Position station, Position position)
        {
            if ((Math.Abs(station.X - position.X) <= distance) && 
                (Math.Abs(station.Y - position.Y) <= distance))
                return true;
            if ((100 - Math.Abs(station.X - position.X) <= distance) &&
                (100 - Math.Abs(station.Y - position.Y) <= distance))
                return true;
            return false;
        }
        public static bool IsCellValid(Position position)
        {
            if (position.X > -1 && position.X < 100)
                if (position.Y > -1 && position.Y < 100)
                    return true;
            return false;
        }
        public static bool IsCellFree(Position cell, IList<Robot.Common.Robot> robots, Robot.Common.Robot movingRobot)
        {
            foreach (var robot in robots)
            {
                if (robot.Position != movingRobot.Position)
                {
                    if (robot.Position == cell)
                        return false;
                }
            }
            return true;
        }
        public static bool IsStationFree(EnergyStation station, IList<Robot.Common.Robot> robots, Robot.Common.Robot movingRobot)
        {
            foreach (var robot in robots)
                if ((robot.Position != movingRobot.Position) && Helper.CanCollect(station.Position, robot.Position))
                    return false;
            return true;
        }
        public static bool IsCollecting(Position position, IEnumerable<EnergyStation> sortedStations)
        {
            foreach (var station in sortedStations)
                if (Helper.CanCollect(station.Position, position))
                    return true;
            return false;
        }
    }
}
