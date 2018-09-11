using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakizh.Taras.RobotChallenge
{
    public class RobotMoving
    {
        private Robot.Common.Robot movingRobot;
        private IList<Robot.Common.Robot> robots;

        public RobotMoving(Robot.Common.Robot robot, IList<Robot.Common.Robot> _robots)
        {
            movingRobot = robot;
            robots = _robots;
        }

        public Position GetNextPosition(Position position, out int steps)
        {
            Position result = position.Copy();
            steps = 1;
            while (true)
            {
                int energySpend = Helper.FindDistance(result, movingRobot.Position) * steps;
                if (movingRobot.Energy < energySpend)
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
            if (way == 0) way = 1;
            while (length > way)
            {
                if (Math.Abs(position.X - movingRobot.Position.X) > Math.Abs(position.Y - movingRobot.Position.Y))
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
                    if (Helper.IsCellFree(position, robots, movingRobot) || way == 1)
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
    }
}
