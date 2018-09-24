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


            //input
            Position[] StartPositions = new Position[]
            {
                new Position(10, 98),
                new Position(0, 0),
                new Position(10, 10),
                new Position(99, 0)
            };
            Position[] EndPositions = new Position[]
            {
                new Position(10, 3),
                new Position(98, 98),
                new Position(10, 10),
                new Position(0, 99)
            };

            //expected
            int[] expected = new int[]
            {
                25, 8, 0, 2
            };

            //Assert
            for(int i = 0; i < StartPositions.Length; ++i)
            {
                Assert.AreEqual(expected[i], Helper.FindDistance(StartPositions[i], EndPositions[i]));
            }
        }

        [TestMethod()]
        public void CanCollectTest()
        {
            Position position1 = new Position(10, 10);
            Position position2 = new Position(13, 13);
            Assert.IsTrue(Helper.CanCollect(position1, position2));

            //input
            Position[] positions = new Position[]
            {
                new Position(10, 10),
                new Position(15, 5),
                new Position(3, 3)
            };
            Position[] stations = new Position[]
            {
                new Position(13, 13),
                new Position(15, 5),
                new Position(99, 99)
            };

            //expected
            bool[] expected = new bool[]
            {
                true, true, false
            };

            //Assert
            for(int i = 0; i < positions.Length; ++i)
            {
                Assert.AreEqual(expected[i], Helper.CanCollect(positions[i], stations[i]));
            }
        }
    }
}