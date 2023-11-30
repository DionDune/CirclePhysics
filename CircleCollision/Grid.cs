using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleCollision
{
    internal class Grid
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int SlotWidth { get; set; }
        public int SlotHeight { get; set; }

        public int SlotCount_X { get; set; }
        public int SlotCount_Y { get; set; }

        public Grid()
        {
            X = -1000;
            Y = -1000;

            SlotWidth = 15;
            SlotHeight = 15;

            SlotCount_X = 500;
            SlotCount_Y = 500;
        }
    }
}
