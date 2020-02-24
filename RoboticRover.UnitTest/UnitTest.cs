using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboticRover.Business.Business;

namespace RoboticRover.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestScenario_1()
        {
            // Arrange
            var _business = new Lazy<RoboticRoverBusiness>(() => { return new RoboticRoverBusiness(); }, true);
            var roverRobotAndMovements = _business.Value.GetRoverRobotDataFromJson(0);
            var result = _business.Value.OperateRoverRobotFromJsonData(roverRobotAndMovements);

            // Act 
            var actualOutput = ($"{result.RoverRobotAndMovements.RoverRobot.Position.CoordinateX} {result.RoverRobotAndMovements.RoverRobot.Position.CoordinateY} {result.RoverRobotAndMovements.RoverRobot.FacingDirection.ToString()}");
            var expectedOutput = "1 3 N";

            // Assert
            Assert.IsTrue(Convert.ToBoolean(result.Result));
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void TestScenario_2()
        {
            // Arrange
            var _business = new Lazy<RoboticRoverBusiness>(() => { return new RoboticRoverBusiness(); }, true);
            var roverRobotAndMovements = _business.Value.GetRoverRobotDataFromJson(1);
            var result = _business.Value.OperateRoverRobotFromJsonData(roverRobotAndMovements);

            // Act 
            var actualOutput = ($"{result.RoverRobotAndMovements.RoverRobot.Position.CoordinateX} {result.RoverRobotAndMovements.RoverRobot.Position.CoordinateY} {result.RoverRobotAndMovements.RoverRobot.FacingDirection.ToString()}");
            var expectedOutput = "5 1 E";

            // Assert
            Assert.IsTrue(Convert.ToBoolean(result.Result));
            Assert.AreEqual(expectedOutput, actualOutput);

        }
    }
}
