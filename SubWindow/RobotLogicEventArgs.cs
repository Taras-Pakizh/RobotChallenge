using Robot.Common;
using System;
using System.Text;

namespace SubWindow
{
    public class RobotLogicEventArgs : EventArgs
    {
        public Position station { get; set; }
        public int RobotId { get; set; }
        public Robot.Common.Robot robot { get; set; }
        public RobotEvents robotEvent { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            switch (robotEvent)
            {
                case RobotEvents.TargetAdded:
                    builder.Append("Target added: ");
                    break;
                case RobotEvents.TargetRemoved:
                    builder.Append("Target removed: ");
                    break;
                case RobotEvents.PropertyAdded:
                    builder.Append("Property added: ");
                    break;
            }
            builder.Append("RobotId = " + RobotId);
            builder.Append(" Robot position: " + robot.Position + " Robot energy: " + robot.Energy);
            builder.Append(" Station: " + station);
            return builder.ToString();
        }

        public enum RobotEvents
        {
            TargetAdded,
            TargetRemoved,
            PropertyAdded
        }
    }
}
