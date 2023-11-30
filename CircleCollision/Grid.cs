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

        public List<List<bool?>> Slots { get; set; }

        public Grid()
        {
            X = -100;
            Y = -100;

            SlotWidth = 30;
            SlotHeight = 30;

            SlotCount_X = 72;
            SlotCount_Y = 44;
        }

        public void GenerateSlots()
        {
            Random random = new Random();

            Slots = new List<List<bool?>>();

            for (int y = 0; y < SlotCount_Y; y++)
            {
                Slots.Add(new List<bool?>());

                for (int x = 0; x < SlotCount_X; x++)
                {
                    if (random.Next(0, 3) == 0)
                    {
                        Slots.Last().Add(true);
                    }
                    else
                    {
                        Slots.Last().Add(null);
                    }
                }
            }
        }
    }
}
