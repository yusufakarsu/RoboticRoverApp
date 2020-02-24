using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticRover.Business.Business;
using RoboticRover.Business.Interfaces;

namespace RoboticRover.App
{
    
    public class Program
    {
        static void Main(string[] args)
        {
            var business = new RoboticRoverBusiness();

            var dataList = business.GetRoverRobotDataListFromJson();
            if (dataList != null && dataList.Any())
            {
                foreach (var item in dataList)
                {
                    var result = business.OperateRoverRobotFromJsonData(item);
                    if (result.Result == 1)
                    {
                        Console.WriteLine($"Data Object {dataList.IndexOf(item) + 1}");
                        Console.WriteLine($"Rover Robot Landed SurfaceLength are : {result.RoverRobotAndMovements.RoverRobot.LandedPlateu.XSurfaceLength} {result.RoverRobotAndMovements.RoverRobot.LandedPlateu.YSurfaceLength} And it's Facing Direction is {result.RoverRobotAndMovements.RoverRobot.FacingDirection.ToString()}");
                        Console.WriteLine($"Rover Robot Positions are : {result.RoverRobotAndMovements.RoverRobot.Position.CoordinateX} {result.RoverRobotAndMovements.RoverRobot.Position.CoordinateY} And it's Facing Direction is {result.RoverRobotAndMovements.RoverRobot.FacingDirection.ToString()}");
                        Console.WriteLine($"Rover Robot Movements are : {string.Join("", result.RoverRobotAndMovements.Movements.Select(x => string.Concat(Enumerable.Repeat(x.Rotation.ToString(), x.Count))).ToList())}");
                        Console.WriteLine($" After the Rover Operation; Rover Robot Robot Positions are {result.RoverRobotAndMovements.RoverRobot.Position.CoordinateX} {result.RoverRobotAndMovements.RoverRobot.Position.CoordinateY} {result.RoverRobotAndMovements.RoverRobot.FacingDirection.ToString()}");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($"Data Object {dataList.IndexOf(item) + 1}");
                        Console.WriteLine($" {result.ResultMessage}");
                    }
                }
            }
            else
                Console.WriteLine($"!!! Missing Data Object");

            Console.ReadLine();
        }
    }
}
