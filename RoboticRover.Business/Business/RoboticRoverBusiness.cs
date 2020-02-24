using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticRover.Business.Interfaces;
using RoboticRover.Data.Interfaces;
using RoboticRover.Global.Models;
using RoboticRover.Global.Enums;
using RoboticRover.Data.Repositories;

namespace RoboticRover.Business.Business
{
    public class RoboticRoverBusiness : IRoboticRoverBusiness
    {
        private readonly Lazy<RoboticRoverData> _data;


        public RoboticRoverBusiness()
        {
            _data = new Lazy<RoboticRoverData>(() => { return new RoboticRoverData(); }, true);
        }

        public RoverOperationResult OperateRoverRobotFromJsonData(RoverRobotAndMovements roverRobotAndMovements)
        {
            var resultObj = new RoverOperationResult();
            try
            {
                if (roverRobotAndMovements != null && roverRobotAndMovements.Movements != null && roverRobotAndMovements.Movements.Any() && roverRobotAndMovements.RoverRobot != null)
                {
                    foreach (var movement in roverRobotAndMovements.Movements)
                    {
                        switch (movement.Rotation)
                        {
                            case Rotations.M:
                                roverRobotAndMovements.RoverRobot.Position = MoveForward(roverRobotAndMovements.RoverRobot.Position, roverRobotAndMovements.RoverRobot.FacingDirection, movement.Count);
                                break;
                            case Rotations.L:
                                roverRobotAndMovements.RoverRobot.FacingDirection = RotateLeft(roverRobotAndMovements.RoverRobot.FacingDirection);
                                break;
                            case Rotations.R:
                                roverRobotAndMovements.RoverRobot.FacingDirection = RotateRight(roverRobotAndMovements.RoverRobot.FacingDirection);
                                break;
                            default:
                                resultObj.Result = -1;
                                resultObj.ResultMessage = ($"Invalid Movement {movement.Rotation.ToString()}");
                                break;
                        }

                        if (roverRobotAndMovements.RoverRobot.Position.CoordinateX < 0 || roverRobotAndMovements.RoverRobot.Position.CoordinateX > roverRobotAndMovements.RoverRobot.LandedPlateu.XSurfaceLength || roverRobotAndMovements.RoverRobot.Position.CoordinateY < 0 || roverRobotAndMovements.RoverRobot.Position.CoordinateY > roverRobotAndMovements.RoverRobot.LandedPlateu.YSurfaceLength)
                        {
                            resultObj.Result = -1;
                            resultObj.ResultMessage = $"!!! Position exceeded plateau limits (0 , 0) and ({roverRobotAndMovements.RoverRobot.LandedPlateu.XSurfaceLength} , {roverRobotAndMovements.RoverRobot.LandedPlateu.YSurfaceLength})";
                            break;
                        }
                    }
                }
                else
                {
                    resultObj.Result = -1;
                    resultObj.ResultMessage = $"!!! There is Missing Information Of RoverRobot";
                }

                if (resultObj.Result != -1)
                {
                    resultObj.Result = 1;
                    resultObj.ResultMessage = "Succes";
                    resultObj.RoverRobotAndMovements = roverRobotAndMovements;
                }
            }
            catch (Exception ex)
            {

            }
            return resultObj;
        }

        public List<RoverRobotAndMovements> GetRoverRobotDataListFromJson()
        {
            var listRoverRobotAndMovements = _data.Value.GetListData();
            return listRoverRobotAndMovements;
        }

        public RoverRobotAndMovements GetRoverRobotDataFromJson(int index = 0)
        {
            var roverRobotAndMovements = _data.Value.GetData(index);
            return roverRobotAndMovements;
        }


        private Position MoveForward(Position position, Directions facingDirection, int Count)
        {
            switch (facingDirection)
            {
                case Directions.N:
                    position.CoordinateY += Count;
                    break;
                case Directions.S:
                    position.CoordinateY -= Count;
                    break;
                case Directions.E:
                    position.CoordinateX += Count;
                    break;
                case Directions.W:
                    position.CoordinateX -= Count;
                    break;
                default:
                    break;
            }
            return position;
        }
        private Directions RotateLeft(Directions facingDirection)
        {
            switch (facingDirection)
            {
                case Directions.N:
                    facingDirection = Directions.W;
                    break;
                case Directions.S:
                    facingDirection = Directions.E;
                    break;
                case Directions.E:
                    facingDirection = Directions.N;
                    break;
                case Directions.W:
                    facingDirection = Directions.S;
                    break;
                default:
                    break;
            }
            return facingDirection;
        }

        private Directions RotateRight(Directions facingDirection)
        {
            switch (facingDirection)
            {
                case Directions.N:
                    facingDirection = Directions.E;
                    break;
                case Directions.S:
                    facingDirection = Directions.W;
                    break;
                case Directions.E:
                    facingDirection = Directions.S;
                    break;
                case Directions.W:
                    facingDirection = Directions.N;
                    break;
                default:
                    break;
            }
            return facingDirection;
        }

    }
}
