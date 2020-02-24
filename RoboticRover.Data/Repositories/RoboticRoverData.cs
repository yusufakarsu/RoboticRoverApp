using Newtonsoft.Json.Linq;
using RoboticRover.Data.Helper;
using RoboticRover.Data.Interfaces;
using RoboticRover.Global.Enums;
using RoboticRover.Global.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticRover.Data.Repositories
{
    public class RoboticRoverData : IRoboticRoverData
    {
        public RoverRobotAndMovements GetData(int defaultValue = 0)
        {
            var filePath = Path.GetFullPath(System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString()).ToString()  + @ConfigurationHelper.DataPath);
            var robotProps = GetJRobotData(filePath);
            var roverRobotAndMovements = new RoverRobotAndMovements();

            if (robotProps != null)
            {
                var listData = MapRoverRobotListData(robotProps);
                if (listData != null && listData.Any() && listData[defaultValue] != null)
                    roverRobotAndMovements = listData[defaultValue];
            }

            return roverRobotAndMovements;
        }

        public List<RoverRobotAndMovements> GetListData()
        {
            var filePath = Path.GetFullPath(System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString()).ToString() + @ConfigurationHelper.DataPath);
            var robotProps = GetJRobotData(filePath);
            var listData = new List<RoverRobotAndMovements>();

            if (robotProps != null)
                listData = MapRoverRobotListData(robotProps);

            return listData;
        }
        private List<RoverRobotAndMovements> MapRoverRobotListData(JObject jsonObj)
        {
            var strPlateuSurfaces = string.Empty;
            var strRobotPosition = string.Empty;
            var strRobotMovements = string.Empty;
            var roverRobotAndMovementsList = new List<RoverRobotAndMovements>();

            if (jsonObj["Inputs"] != null)
            {
                var inputs = jsonObj["Inputs"].ToList();

                foreach (var item in inputs)
                {
                    var movementList = new List<Movement>();
                    var roverRobotAndMovements = new RoverRobotAndMovements()
                    {
                        RoverRobot = new RoverRobot
                        {
                            FacingDirection = Directions.N,
                            LandedPlateu = new Plateu { XSurfaceLength = 0, YSurfaceLength = 0 },
                            Position = new Position { CoordinateX = 0, CoordinateY = 0 }
                        }
                    };
                    strPlateuSurfaces = item[0].ToString();
                    strRobotPosition = item[1].ToString();
                    strRobotMovements = item[2].ToString();

                    if (!string.IsNullOrEmpty(strPlateuSurfaces))
                    {
                        var strPlateuSurfaceList = strPlateuSurfaces.Trim().Split(' ');
                        if (strPlateuSurfaceList != null && strPlateuSurfaceList.Any() && strPlateuSurfaceList.Count() == 2)
                        {
                            roverRobotAndMovements.RoverRobot.LandedPlateu.XSurfaceLength = Convert.ToInt32(strPlateuSurfaceList[0]);
                            roverRobotAndMovements.RoverRobot.LandedPlateu.YSurfaceLength = Convert.ToInt32(strPlateuSurfaceList[1]);
                        }
                    }

                    if (!string.IsNullOrEmpty(strRobotPosition))
                    {
                        var strRobotPositionList = strRobotPosition.Trim().Split(' ');
                        if (strRobotPositionList != null && strRobotPositionList.Any() && strRobotPositionList.Count() == 3)
                        {
                            roverRobotAndMovements.RoverRobot.Position.CoordinateX = Convert.ToInt32(strRobotPositionList[0]);
                            roverRobotAndMovements.RoverRobot.Position.CoordinateY = Convert.ToInt32(strRobotPositionList[1]);
                            roverRobotAndMovements.RoverRobot.FacingDirection = (Directions)Enum.Parse(typeof(Directions), strRobotPositionList[2]);
                        }
                    }

                    if (!string.IsNullOrEmpty(strRobotMovements))
                    {
                        var strRobotMovementList = strRobotMovements.ToUpper().Trim().ToCharArray();

                        if (strRobotMovementList != null && strRobotMovementList.Any())
                        {
                            var movementStep = 0;
                            foreach (var movementItem in strRobotMovementList)
                            {
                                var movement = new Movement();

                                var currentRotation = (Rotations)Enum.Parse(typeof(Rotations), movementItem.ToString());

                                if (movementList != null && movementList.Any() && (movementList.LastOrDefault().Rotation == currentRotation && currentRotation == Rotations.M))
                                {
                                    movementList.LastOrDefault().Count++;
                                }
                                else
                                {
                                    movementStep++;
                                    movement.Rotation = currentRotation;
                                    movement.Step = movementStep;
                                    movement.Count = 1;
                                    movementList.Add(movement);
                                }
                            }
                            roverRobotAndMovements.Movements = movementList;
                        }
                    }

                    roverRobotAndMovementsList.Add(roverRobotAndMovements);
                }
            }
            return roverRobotAndMovementsList;
        }
        private JObject GetJRobotData(string path)
        {
            var resultObject = new JObject();
            try
            {
                string jsonString = File.ReadAllText(path);
                resultObject = JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {

            }
            return resultObject;
        }
    }
}
