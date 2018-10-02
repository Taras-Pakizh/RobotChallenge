using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakizh.Taras.RobotChallenge
{
    public class StepEventArgs : EventArgs
    {
        public Position PrevPosition { get; set; }
        public int CurrentPosition { get; set; }
        public RobotMoving.Axis axis { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Previous position: " + PrevPosition.ToString());
            var cPosition = new Position();
            if(axis == RobotMoving.Axis.X)
            {
                cPosition.X = CurrentPosition;
                cPosition.Y = PrevPosition.Y;
            }
            else
            {
                cPosition.X = PrevPosition.X;
                cPosition.Y = CurrentPosition;
            }
            builder.Append("Current position: " + cPosition.ToString());
            return builder.ToString();
        }
    }
}
