using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;


namespace River
{
    static class Tile
    {
        private const int FloorTileCount = 12;
        private const int HeightTileCount = 7;
        private const int SurfaceTileCount = 11;
        static public Texture2D[] FloorTiles = new Texture2D[FloorTileCount];
        static public Texture2D[] HeightTiles = new Texture2D[HeightTileCount];
        static public Texture2D[] SurfaceTiles = new Texture2D[SurfaceTileCount];

        public const int TextureTilesAcross = 10;

        public const int TileWidth = 128;
        public const int TileHeight = 128;
        public const int TileStepX = 128;
        public const int TileStepY = 32;
        public const int OddRowXOffset = TileWidth / 2;
        public const int HeightTileOffset = TileHeight / 2;

        static public void LoadContent(ContentManager Content)
        {
            for (int ecx = 0; ecx < FloorTileCount; ecx++)
            {
                FloorTiles[ecx] = Content.Load<Texture2D>(@"Textures\Tiles\FloorTiles\FloorTile" + ecx.ToString());
            }

            for (int ecx = 0; ecx < HeightTileCount; ecx++)
                HeightTiles[ecx] = Content.Load<Texture2D>(@"Textures\Tiles\HeightTiles\HeightTile" + ecx.ToString());


            for (int ecx = 0; ecx < SurfaceTileCount; ecx++)
                SurfaceTiles[ecx] = Content.Load<Texture2D>(@"Textures\Tiles\SurfaceTiles\SurfaceTile" + ecx.ToString());
        }

        /*
        static public Rectangle GetSourceRectangle(int TileIndex)
        {
            int TileY = TileIndex / (TileSetTexture.Width / TileWidth);
            int TileX = TileIndex % (TileSetTexture.Width / TileWidth);

            return new Rectangle(TileX * TileWidth, TileY * TileHeight, TileWidth, TileHeight);
        }
        */

        static public bool IntersectionTest(Vector2 ComparePosition, Vector2 Origin, float Radius)
        {
            //SQUARE CHECK

            //X bounds first (no extra calculations needed, easiest to do)
            if (ComparePosition.X >= Origin.X - Radius &&
                ComparePosition.X <= Origin.X + Radius)
            {
                //Y direction will be half as far (aspect ratio)
                if ((Origin.Y - ComparePosition.Y) * 2 <= Radius &&
                    (ComparePosition.Y - Origin.Y) * 2 <= Radius)
                {
                    //CIRCLE CHECK
                    if ((float)Math.Sqrt(Math.Pow(ComparePosition.X - Origin.X, 2) +
                        Math.Pow((ComparePosition.Y - Origin.Y) * 2, 2)) <= Radius)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //Just the square check alone
        static public bool SquareTest(Vector2 ComparePosition, Vector2 Origin, float Radius)
        {
            //SQUARE CHECK

            //X bounds first (no extra calculations needed, easiest to do)
            if (ComparePosition.X >= Origin.X - Radius &&
                ComparePosition.X <= Origin.X + Radius)
            {
                //Y direction will be half as far (aspect ratio)
                if ((Origin.Y - ComparePosition.Y) * 2 <= Radius &&
                    (ComparePosition.Y - Origin.Y) * 2 <= Radius)
                {
                    return true;
                }
            }

            return false;
        }

        //Just the circle check alone that 'outs' a distance
        static public bool CircleTest(Vector2 ComparePosition, Vector2 Origin, float Radius, out float Distance)
        {
            Distance = (float)Math.Sqrt(Math.Pow(ComparePosition.X - Origin.X, 2) +
                        Math.Pow((ComparePosition.Y - Origin.Y) * 2, 2));

            if (Distance <= Radius)
                return true;

            return false;
        }

    }
}
