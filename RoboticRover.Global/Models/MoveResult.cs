using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticRover.Global.Models
{
    public class MoveResult
    {
        public Position Position { get; set; }
        public Position skippedPosition { get; set; }
        public bool Result  { get; set; }
        public int MovementStep { get; set; }

        public bool isSkipped { get; set; }
}
}
