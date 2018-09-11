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
        public void GetNextPositionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DivideWayBy2Test()
        {
            Assert.Fail();
        }
    }
}