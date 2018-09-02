using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pakizh.Taras.RobotChallenge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

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
                    new Robot.Common.Robot(){Position = new Position(69,81)},
                    new Robot.Common.Robot(){Position = new Position(55,92)},
                    new Robot.Common.Robot(){Position = new Position(76,91)},
                    new Robot.Common.Robot(){Position = new Position(70,79)},
                    new Robot.Common.Robot(){Position = new Position(49,99)},
                    new Robot.Common.Robot(){Position = new Position(62,98)},
                    new Robot.Common.Robot(){Position = new Position(60,85)},
                    new Robot.Common.Robot(){Position = new Position(59,93)},
                    new Robot.Common.Robot(){Position = new Position(62,97)},
                    new Robot.Common.Robot(){Position = new Position(83,81)},
                },
                sortedStations = new EnergyStation[]
                {
                    new EnergyStation() { Position = new Position(85, 70) }
                }
            };
        }

        [TestMethod()]
        public void PakizhTarasAlgorithmTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PakizhTarasAlgorithmTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DoStepTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetNextPositionTest()
        {
            Position finish = new Position(51, 50);
            pakizh.movingRobot.Position = new Position(25, 25);
            pakizh.movingRobot.Energy = 50;
            Assert.AreEqual(pakizh.movingRobot.Position, pakizh.GetNextPosition(finish, out int steps));
        }

        [TestMethod()]
        public void DivideWayBy2Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindNearestFreeStationTest()
        {
            Assert.IsNotNull(pakizh.FindNearestFreeStation());
        }

        [TestMethod()]
        public void FindNearestOccupiedStationTest()
        {
            Assert.IsNotNull(pakizh.FindNearestOccupiedStation());
        }

        [TestMethod()]
        public void FindNearestFreeCellAroundStationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsCellFreeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsStationFreeTest()
        {
            Assert.AreEqual(true, pakizh.IsStationFree(pakizh.sortedStations[0]));
        }

        [TestMethod()]
        public void IsCollectingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsCellValidTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InitTest()
        {
            Assert.Fail();
        }
    }
}