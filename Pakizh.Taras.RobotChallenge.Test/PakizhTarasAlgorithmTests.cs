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
            var myRobot = new Robot.Common.Robot() { Position = new Position(20, 20), Energy = 100000 };
            pakizh = new PakizhTarasAlgorithm()
            {
                movingRobot = myRobot,
                robots = new List<Robot.Common.Robot>()
                {
                    myRobot,
                    new Robot.Common.Robot(){Position = new Position(42, 10)},
                    new Robot.Common.Robot(){Position = new Position(30, 30)},
                    new Robot.Common.Robot(){Position = new Position(10, 30)},
                    new Robot.Common.Robot(){Position = new Position(5, 25)},
                },
                sortedStations = new EnergyStation[0],
                TargetBook = new Dictionary<int, Position>(),
                PropertyBook = new Dictionary<int, Position>(),
                MyRobotId = 0,
            };
        }

        [TestMethod]
        public void ChooseFreeTest()
        {
            pakizh.sortedStations = new EnergyStation[]
            {
                new EnergyStation() { Position = new Position(45, 10) },
                new EnergyStation() { Position = new Position(30, 30) },
                new EnergyStation() { Position = new Position(5, 18) },
            };
            pakizh.map = new Map()
            {
                Stations = pakizh.sortedStations
            };

            var expected = new Position(8, 20);
            Assert.AreEqual(expected, pakizh.GetMoveCommand().NewPosition);
        }

        [TestMethod]
        public void ChooseOccupiedTest()
        {
            pakizh.sortedStations = new EnergyStation[]
            {
                new EnergyStation() { Position = new Position(40, 10) },
                new EnergyStation() { Position = new Position(33, 30) },
                new EnergyStation() { Position = new Position(5, 25) },
            };
            pakizh.map = new Map()
            {
                Stations = pakizh.sortedStations
            };

            var expected = new Position(37, 13);
            Assert.AreEqual(expected, pakizh.GetMoveCommand().NewPosition);
        }

        [TestMethod]
        public void TargetBookTest()
        {
            pakizh.sortedStations = new EnergyStation[]
            {
                new EnergyStation() { Position = new Position(30, 30) },
                new EnergyStation() { Position = new Position(45, 30) },
            };
            pakizh.map = new Map()
            {
                Stations = pakizh.sortedStations
            };
            pakizh.robots.Add(new Robot.Common.Robot()
            {
                Position = new Position(42, 27)
            });

            pakizh.TargetBook.Clear();
            pakizh.TargetBook.Add(0, new Position(45, 30));
            var expected = new Position(42, 28);
            Assert.AreEqual(expected, pakizh.GetMoveCommand().NewPosition);
        }

        [TestMethod]
        public void PropertyBookTest()
        {
            pakizh.sortedStations = new EnergyStation[]
            {
                new EnergyStation() { Position = new Position(25, 20) },
                new EnergyStation() { Position = new Position(25, 10) },
            };
            pakizh.map = new Map()
            {
                Stations = pakizh.sortedStations
            };
            pakizh.robots.Add(new Robot.Common.Robot()
            {
                Position = new Position(23, 13)
            });

            pakizh.PropertyBook.Clear();
            pakizh.PropertyBook.Add(0, new Position(25, 10));
            var expected = new Position(22, 13);
            Assert.AreEqual(expected, pakizh.GetMoveCommand().NewPosition);
        }

        [TestMethod]
        public void GetCollectEnergyCommandTest()
        {
            pakizh.sortedStations = new EnergyStation[]
            {
                new EnergyStation(){Position = new Position(23, 23)},
            };
            var command = pakizh.GetCollectEnergyCommand();
            Assert.IsInstanceOfType(command, typeof(CollectEnergyCommand));

            pakizh.sortedStations = new EnergyStation[]
            {
                new EnergyStation(){Position = new Position(25, 25)}
            };
            pakizh.PropertyBook.Clear();
            pakizh.PropertyBook.Add(0, new Position(25, 25));
            command = pakizh.GetCollectEnergyCommand();
            Assert.IsNull(command);
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