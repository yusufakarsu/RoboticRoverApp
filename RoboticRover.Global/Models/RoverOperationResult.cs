using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticRover.Global.Models
{
    public class RoverOperationResult
    {
        public RoverRobotAndMovements RoverRobotAndMovements { get; set; }
        public List<Position> SkippedPositions { get; set; }
        public int Result { get; set; }
        public string ResultMessage { get; set; }
    }
}
