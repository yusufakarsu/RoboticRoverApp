
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticRover.Data;
using RoboticRover.Global.Models;

namespace RoboticRover.Business.Interfaces
{
    public interface IRoboticRoverBusiness
    {
        RoverOperationResult OperateRoverRobotFromJsonData(RoverRobotAndMovements roverRobotAndMovements);

        List<RoverRobotAndMovements> GetRoverRobotDataListFromJson();

        RoverRobotAndMovements GetRoverRobotDataFromJson(int index = 0);

    }
}
