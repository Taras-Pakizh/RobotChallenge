using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;
using System.Collections.Generic;

namespace Pakizh.Taras.RobotChallenge.Tests
{
    [TestClass]
    public class PakizhTarasAlgorithmTests
    {
        private PakizhTarasAlgorithm pakizh;

        [TestInitialize]
        public void SetValues()
        {
            pakizh = new PakizhTarasAlgorithm()
            {
                movingRobot = new Robot.Common.Robot() { Position = new Position(62, 98) },
                robots = new List<Robot.Common.Robot>()
                {
                    new Robot.Common.Robot() { Position = new Position(62, 98) },
                },
                sortedStations = new EnergyStation[]
                {
                    new EnergyStation() { Position = new Position(85, 70) }
                }
            };
        }

        [TestMethod()]
        public void DoStepCreateRobotTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DoStepCollectEnergyTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DoStepMoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindNearestFreeCellAroundStationTest()
        {
            //input
            Position[] stations = new Position[]
            {
                new Position(10, 10),
                new Position(10, 10),
                new Position(16, 10),
                //
                new Position(98, 98),
                new Position(98, 1),
                new Position(50, 50),
            };
            Position[] start = new Position[]
            {
                new Position(20, 20),
                new Position(12, 12),
                new Position(14, 5),
                new Position(0, 0),
                new Position(5, 2),
                new Position(30, 20),
            };

            //expected
            Position[] expected = new Position[]
            {
                new Position(13, 13),
                new Position(12, 12),
                new Position(14, 7),
                new Position(99, 99),
                new Position(99, 2),
                new Position(47, 47),
            };

            //Assert
            for(int i = 0; i < stations.Length; ++i)
            {
                pakizh.movingRobot.Position = start[i];
                Assert.AreEqual(expected[i], pakizh.FindNearestFreeCellAroundStation(new EnergyStation() { Position = stations[i] }));
            }
        }
    }
}