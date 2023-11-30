using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleCollision
{
    internal class Circle
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float Radius { get; set; }
        public float Diameter { get; set; }

        public float MovementAngle { get; set; }
        public float MovementVelocity { get; set; }
        public float VelocityDecayMultiplier { get; set; }


        public Color Color { get; set; }
        public bool Selected { get; set; }

        public bool isColliding;




        public void ChangeAngle(float Change)
        {
            MovementAngle += Change;

            if (MovementAngle < 0)
            {
                MovementAngle += 360;
            }
            else if (MovementAngle >= 360)
            {
                MovementAngle -= 360;
            }
        }
        public void ChangeSpeed(float Change)
        {
            //MovementVelocity += Change;
            if (Change < 0)
            {
                MovementVelocity = -6;
            }
            else
            {
                MovementVelocity = 6;
            }

        }

        public bool isCollidingWith(Circle Circle)
        {
            float Distance = (float)Math.Sqrt(Math.Pow(Math.Abs(X - Circle.X), 2) +
                                                           Math.Pow(Math.Abs(Y - Circle.Y), 2));
            float TotalWidth = Radius + Circle.Radius;


            if (Distance < TotalWidth)
            {

                float Overlap = TotalWidth - Distance;
                if (Overlap >= 1)
                {
                    killOverlap(Circle, Overlap);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        public void killOverlap(Circle Circle, float Overlap)
        {
            //Flip Diff Calculation for "fun"
            float xDiff = X - Circle.X;
            float yDiff = Y - Circle.Y;
            float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

            float AngleRadians = Angle * (float)(Math.PI / 180);

            X += Overlap * (float)Math.Cos(AngleRadians);
            Y += Overlap * (float)Math.Sin(AngleRadians);
        }

        public void EnactVelocity()
        {
            float AngleRadians = MovementAngle * (float)(Math.PI / 180);

            X += MovementVelocity * (float)Math.Cos(AngleRadians);
            Y += MovementVelocity * (float)Math.Sin(AngleRadians);

            MovementVelocity *= VelocityDecayMultiplier;
        }
        public void EnactCollision(Circle Circle)
        {
            float xDiff = X - Circle.X;
            float yDiff = Y - Circle.Y;
            float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

            if (Math.Abs(MovementVelocity) > Math.Abs(Circle.MovementVelocity))
            {
                Circle.MovementAngle = Angle;
                Circle.ChangeAngle(180);
                Circle.MovementVelocity = Math.Abs(MovementVelocity) * 1F;

                MovementVelocity *= 0.0F;
            }
            else
            {
                MovementAngle = Angle + 180;
                ChangeAngle(180);
                MovementVelocity = Math.Abs(Circle.MovementVelocity) * 1F;

                Circle.MovementVelocity *= 0.0F;
            }
        }
    }
}
