using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace River
{
    static class UnitCircle
    {

        public static float GetCircleAngle(Vector2 DirectionVector)
        {
            float Angle = 0f;

            //Just grab an angle based on unit circle crap
            if (DirectionVector.X == 1 && DirectionVector.Y == 0)
                Angle = 0f;
            else if (DirectionVector.X == 1 && DirectionVector.Y == 1)
                Angle = (float)Math.PI / 4;
            else if (DirectionVector.X == 0 && DirectionVector.Y == 1)
                Angle = (float)Math.PI / 2;
            else if (DirectionVector.X == -1 && DirectionVector.Y == 1)
                Angle = (float)Math.PI * 3 / 4;
            else if (DirectionVector.X == -1 && DirectionVector.Y == 0)
                Angle = (float)Math.PI;
            else if (DirectionVector.X == -1 && DirectionVector.Y == -1)
                Angle = (float)Math.PI * 5 / 4;
            else if (DirectionVector.X == 0 && DirectionVector.Y == -1)
                Angle = (float)Math.PI * 3 / 2;
            else if (DirectionVector.X == 1 && DirectionVector.Y == -1)
                Angle = (float)Math.PI * 7 / 4;

            return Angle;
        }

        /// <summary>
        /// Returns a vector2 that represents the vector between two points shortened to fit on the unit circle
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Destination"></param>
        /// <returns></returns>
        public static Vector2 ComputeAngle(Vector2 Source, Vector2 Destination)
        {
            Vector2 MiddleVector = new Vector2();

            float Angle;
            if (Source.X - Destination.X != 0)
                Angle = (float)Math.Atan((Source.Y - Destination.Y) / (Source.X - Destination.X));
            else
                Angle = (float)Math.PI * 3 / 2; //Manually set angle when otherwise would be dividing by 0

            //Use that angle to create a vector where components cannot exceed 1
            MiddleVector.X = (float)Math.Cos(Angle);
            //Since MoveVector.X is equal to the cos value, use the cos^2(x)+sin^2(x)=1 trig identity to find sin (much faster)
            //MoveVector.Y = (float)Math.Sin(Angle); //Old
            MiddleVector.Y = (float)Math.Sqrt(1 - MiddleVector.X * MiddleVector.X);

            if (Source.X >= Destination.X)
                MiddleVector.X = -MiddleVector.X;
            if (Source.Y >= Destination.Y)
                MiddleVector.Y = -MiddleVector.Y;

            return MiddleVector;
        }

    }
}
