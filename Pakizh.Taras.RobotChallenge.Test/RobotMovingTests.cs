using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pakizh.Taras.RobotChallenge.Tests
{
    [TestClass()]
    public class RobotMovingTests
    {
        private PakizhTarasAlgorithm pakizh;

        [TestInitialize]
        public void TestInitialize()
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
        public void GetNextPositionTest()
        {
            //input
            Position[] starts = new Position[]
            {
                new Position(62, 98),
                new Position(50, 50),
                new Position(50, 50),
                new Position(0, 0),
                new Position(5, 5),
            };
            int[] energy = new int[]
            {
                100,
                20,
                20,
                50,
                100,
            };
            Position[] stations = new Position[]
            {
                new Position(62, 3),
                new Position(50, 70),
                new Position(50, 71),
                new Position(5, 5),
                new Position(5, 95),
            };

            //expected
            Position[] expected = new Position[]
            {
                new Position(62, 3),
                new Position(50, 51),
                new Position(50, 50),
                new Position(5, 5),
                new Position(5, 95),
            };

            var test = new RobotMoving(pakizh.movingRobot, pakizh.robots);

            //Assert
            for(int i = 0, steps = 0; i < starts.Length; ++i)
            {
                pakizh.movingRobot.Position = starts[i];
                pakizh.movingRobot.Energy = energy[i];
                Assert.AreEqual(expected[i], test.GetNextPosition(stations[i], out steps));
            }
        }

        [TestMethod()]
        public void DivideWayBy2Test()
        {
            //input
            Position[] start = new Position[]
            {
                new Position(10, 10),
                new Position(30, 10),
                new Position(0, 0),
                new Position(30, 10),
                new Position(60, 60)
            };
            Position[] stations = new Position[]
            {
                new Position(50, 50),
                new Position(60, 30),
                new Position(90, 90),
                new Position(90, 80),
                new Position(80, 80)
            };

            //expected
            Position[] expected = new Position[]
            {
                new Position(30, 30),
                new Position(43, 22),
                new Position(95, 95),
                new Position(12, 93),
                new Position(70, 69)
            };

            pakizh.robots.Add(new Robot.Common.Robot() { Position = new Position(70, 70) });
            var test = new RobotMoving(pakizh.movingRobot, pakizh.robots);

            //Assert
            for (int i = 0; i < start.Length; ++i)
            {
                pakizh.movingRobot.Position = start[i];
                Assert.AreEqual(expected[i], test.DivideWayBy2(stations[i]));
            }
        }
    }
}