using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticRover.Global;
using RoboticRover.Global.Models;

namespace RoboticRover.Data.Interfaces
{
    public interface IRoboticRoverData
    {
        RoverRobotAndMovements GetData(int defaultValue = 1);
        List<RoverRobotAndMovements> GetListData();
        OccurenceOfErrorPositions GetOccurenceOfErrorList();
        void SetOccurenceOfError(Position position);
    }
}
