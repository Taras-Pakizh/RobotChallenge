using Robot.Common;
using System;
using System.Collections.Generic;

namespace Pakizh.Taras.RobotChallenge
{
    public class RobotMoving
    {
        //Vars
        private Robot.Common.Robot movingRobot;
        private IList<Robot.Common.Robot> robots;

        //Events
        public event StepsHaldler StepDone;

        //Constructors
        public RobotMoving(Robot.Common.Robot robot, IList<Robot.Common.Robot> _robots)
        {
            movingRobot = robot;
            robots = _robots;
        }

        //Methods
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
                    if (result == movingRobot.Position)
                        break;
                    steps = GetDistance(position) / GetDistance(result);
                    continue;
                }
                break;
            }
            return result;
        }
        public Position DivideWayBy2(Position position)
        {
            int length = GetDistance(position.X, movingRobot.Position.X, out bool ReverseX) + 
                GetDistance(position.Y, movingRobot.Position.Y, out bool ReverseY);
            if (length == 1)
                return movingRobot.Position;
            int way = length / 2;
            while (length > way)
            {
                int DistanceX = GetDistance(position.X, movingRobot.Position.X, out ReverseX);
                int DistanceY = GetDistance(position.Y, movingRobot.Position.Y, out ReverseY);

                if (DistanceX > DistanceY)
                {
                    position = DoStep(Axis.X, ref ReverseX, position);
                }
                else
                {
                    position = DoStep(Axis.Y, ref ReverseY, position);
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
        private int GetDistance(int start, int finish, out bool reverse)
        {
            int result = Math.Abs(start - finish);
            reverse = false;
            if(100 - result < result)
            {
                result = 100 - result;
                reverse = true;
            }
            return result;
        }
        private int GetDistance(Position station)
        {
            return  GetDistance(station.X, movingRobot.Position.X, out bool ReverseX) +
                GetDistance(station.Y, movingRobot.Position.Y, out bool ReverseY);
        }
        private Position DoStep(Axis axis, ref bool reverse, Position position)
        {
            int currentPos = 0, currentRobot = 0;
            if (axis == Axis.X)
            {
                currentPos = position.X;
                currentRobot = movingRobot.Position.X;
            }
            else
            {
                currentPos = position.Y;
                currentRobot = movingRobot.Position.Y;
            }

            if (!reverse)
            {
                if (currentPos > currentRobot)
                    currentPos -= 1;
                else currentPos += 1;
            }
            else
            {
                if (currentPos > currentRobot)
                {
                    currentPos += 1;
                    if (currentPos == 100)
                    {
                        currentPos = 0;
                        reverse = false;
                    }
                }
                else
                {
                    currentPos -= 1;
                    if (currentPos == -1)
                    {
                        currentPos = 99;
                        reverse = false;
                    }
                }
            }
            StepDone?.Invoke(this, new StepEventArgs() { CurrentPosition = currentPos, PrevPosition = position, axis = axis });
            if (axis == Axis.X)
                position.X = currentPos;
            else position.Y = currentPos;
            return position;
        }

        //Classes
        public enum Axis
        {
            X,
            Y
        }
    }
}
