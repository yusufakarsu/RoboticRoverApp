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

        private Random random = new Random();

        public RoboticRoverBusiness()
        {
            _data = new Lazy<RoboticRoverData>(() => { return new RoboticRoverData(); }, true);
        }

        public RoverOperationResult OperateRoverRobotFromJsonData(RoverRobotAndMovements roverRobotAndMovements)
        {
            var resultObj = new RoverOperationResult { SkippedPositions = new List<Position>() };
            try
            {
                if (roverRobotAndMovements != null && roverRobotAndMovements.Movements != null && roverRobotAndMovements.Movements.Any() && roverRobotAndMovements.RoverRobot != null)
                {
                    MoveResult moveResult = null;

                    foreach (var movement in roverRobotAndMovements.Movements)
                    {
                        moveResult = null;
                        switch (movement.Rotation)
                        {
                            case Rotations.M:
                                moveResult = MoveForward(roverRobotAndMovements.RoverRobot.Position, roverRobotAndMovements.RoverRobot.FacingDirection, movement.Count);
                                if(moveResult != null)
                                    roverRobotAndMovements.RoverRobot.Position = moveResult.Position;
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

                        if (moveResult != null && moveResult.Result && moveResult.isSkipped)
                        {
                            resultObj.Result = 1;
                            resultObj.SkippedPositions.Add(moveResult.skippedPosition);
                            resultObj.ResultMessage += $"\n !!! Skipped Old Occurenced Error Coordinates ({moveResult.skippedPosition.CoordinateX} , {moveResult.skippedPosition.CoordinateY})";

                        }
                        else if (moveResult != null && !moveResult.Result)
                        {
                            resultObj.Result = -1;
                            resultObj.ResultMessage += $"\n !!! An error occured while the robot was moving On ({roverRobotAndMovements.RoverRobot.Position.CoordinateX} , {roverRobotAndMovements.RoverRobot.Position.CoordinateY}) Coordinates";
                            break;
                        }
                        else if (roverRobotAndMovements.RoverRobot.Position.CoordinateX < 0 || roverRobotAndMovements.RoverRobot.Position.CoordinateX > roverRobotAndMovements.RoverRobot.LandedPlateu.XSurfaceLength || roverRobotAndMovements.RoverRobot.Position.CoordinateY < 0 || roverRobotAndMovements.RoverRobot.Position.CoordinateY > roverRobotAndMovements.RoverRobot.LandedPlateu.YSurfaceLength)
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
                    resultObj.ResultMessage += "\n Succes";
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

        // Yeni Logic için bu kullanım kaldırıldı
        //private Position MoveForward(Position position, Directions facingDirection, int Count)
        //{

        //    switch (facingDirection)
        //    {
        //        case Directions.N:
        //            position.CoordinateY += Count;
        //            break;
        //        case Directions.S:
        //            position.CoordinateY -= Count;
        //            break;
        //        case Directions.E:
        //            position.CoordinateX += Count;
        //            break;
        //        case Directions.W:
        //            position.CoordinateX -= Count;
        //            break;
        //        default:
        //            break;
        //    }
        //    return position;
        //}

        private MoveResult MoveForward(Position position, Directions facingDirection, int MovementCount)
        {

            var result = new MoveResult { MovementStep = 0, Position = position, Result = true, isSkipped = false, skippedPosition = null };
            while (MovementCount > 0 && result.Result)
            {
                MovementCount--;
                result.MovementStep++;

                switch (facingDirection)
                {
                    case Directions.N:
                        result.Position.CoordinateY++;
                        break;
                    case Directions.S:
                        result.Position.CoordinateY--;
                        break;
                    case Directions.E:
                        result.Position.CoordinateX++;
                        break;
                    case Directions.W:
                        result.Position.CoordinateX--;
                        break;
                    default:
                        break;
                }

                if (CheckOldOccurenceOfErrors(result.Position))
                {
                    result.skippedPosition = result.Position;
                    result.isSkipped = true;
                    result.Result = true;
                    break;
                }

                if (!result.isSkipped)
                {
                    result.Result = checkErrorOccurence();
                }

            }

            if (!result.Result)
            {
                _data.Value.SetOccurenceOfError(result.Position);
            }
            return result;
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

        private bool checkErrorOccurence()
        {
            return (random.Next(0, 100) % 7 != 0);
        }

        private bool CheckOldOccurenceOfErrors(Position position)
        {
            var oldErrorOccurences = _data.Value.GetOccurenceOfErrorList();
            var result = false;
            if (oldErrorOccurences != null && oldErrorOccurences.Positions != null && oldErrorOccurences.Positions.Any())
            {
                foreach (var item in oldErrorOccurences.Positions)
                {
                    if (item.CoordinateX == position.CoordinateX && item.CoordinateY == position.CoordinateY)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
