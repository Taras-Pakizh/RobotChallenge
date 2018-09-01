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
            Assert.Fail();
        }

        [TestMethod()]
        public void DivideWayBy2Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindNearestFreeStationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindNearestOccupiedStationTest()
        {
            Assert.Fail();
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
            PakizhTarasAlgorithm pakizh = new PakizhTarasAlgorithm()
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
                }
            };
            Assert.AreEqual(true, pakizh.IsStationFree(new EnergyStation() { Position = new Position(85, 70) }));
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