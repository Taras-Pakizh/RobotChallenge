using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace Pakizh.Taras.RobotChallenge
{
    static class Helper
    {
        public static int distance = 3;
        public static int EnergyToBorn = 350;
        public static int RoundToStop = 40;

        public static int FindDistance(Position a, Position b)
        {
            return (int)(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
        public static bool CanCollect(EnergyStation station, Position position)
        {
            if ((Math.Abs(station.Position.X - position.X) <= distance) && 
                (Math.Abs(station.Position.Y - position.Y) <= distance))
                return true;
            return false;
        }
    }
}
