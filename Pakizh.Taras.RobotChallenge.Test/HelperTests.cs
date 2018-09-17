using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;

namespace Pakizh.Taras.RobotChallenge.Tests
{
    [TestClass()]
    public class HelperTests
    {
        [TestMethod()]
        public void FindDistanceTest()
        {
            Position position1 = new Position(10, 98);
            Position position2 = new Position(10, 3);

            int distance = Helper.FindDistance(position1, position2);
            Assert.AreEqual(25, distance);
        }

        [TestMethod()]
        public void CanCollectTest()
        {
            Position position1 = new Position(10, 10);
            Position position2 = new Position(13, 13);
            Assert.IsTrue(Helper.CanCollect(position1, position2));
        }
    }
}