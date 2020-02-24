using RoboticRover.Global.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticRover.Global.Models
{
    public class RoverRobot
    {
        public Position Position { get; set; }
        public Directions FacingDirection { get; set; }
        public Plateu LandedPlateu { get; set; }
    }
}
