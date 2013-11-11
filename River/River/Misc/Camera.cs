using Microsoft.Xna.Framework;
using System;

namespace River
{
    static class Camera
    {

        public static Vector2 location = Vector2.Zero;
        public static Vector2 Location
        {
            get
            {
                return location;
            }
            set
            {
                //Clamp location between map bounds
                location = new Vector2(
                    MathHelper.Clamp(value.X, -256f, Math.Abs(WorldWidth - Main.BackBufferWidth)),
                    MathHelper.Clamp(value.Y, -256f, Math.Abs(WorldHeight - Main.BackBufferHeight)));
            }
        }

        public static int WorldWidth;
        public static int WorldHeight;

        //public const int XScrollDistance = 2*Main.BackBufferWidth / 5;
        //public const int YScrollDistance = 2*Main.BackBufferHeight / 5;

        public const int XScrollDistance = Main.BackBufferWidth / 2 - 2;
        public const int YScrollDistance = Main.BackBufferHeight / 2 - 2;

        public static Vector2 WorldToScreen(Vector2 WorldPosition)
        {
            //Round to prevent 'black lines' between tiles when camera scrolls
            Vector2 RoundedLocation = new Vector2();
            RoundedLocation.X = (int)Location.X;
            RoundedLocation.Y = (int)Location.Y;

            return WorldPosition - RoundedLocation;
        }

        public static Vector2 ScreenToWorld(Vector2 ScreenPosition)
        {
            return ScreenPosition + Location;
        }

        public static void Move(Vector2 Offset)
        {
            Location += Offset;
        }

    }
}
