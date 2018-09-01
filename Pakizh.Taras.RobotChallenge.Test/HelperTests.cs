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
    public class HelperTests
    {
        [TestMethod()]
        public void FindDistanceTest()
        {
            Position position1 = new Position(10, 10);
            Position position2 = new Position(10, 10);

            int distance = Helper.FindDistance(position1, position2);
            Assert.AreEqual(0, distance);
        }

        [TestMethod()]
        public void CanCollectTest()
        {
            Assert.Fail();
        }
    }
}