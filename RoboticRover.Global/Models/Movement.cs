using RoboticRover.Global.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticRover.Global.Models
{
    public class Movement
    {
        public int Step { get; set; }

        public int Count { get; set; }
        public Rotations Rotation { get; set; }
    }
}
