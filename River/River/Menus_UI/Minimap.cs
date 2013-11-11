using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace River
{
    class Minimap : Menu
    {
        private Texture2D MinimapSquare;
        private Texture2D MapBG;
        private Level LevelPTR;

        private Vector2 PanOffset = Vector2.Zero;

        private Vector2 MapDrawPos;

        public Minimap(Level LevelPTR, MenuSideType MenuSideType)
            : base(MenuSideType)
        {
            this.LevelPTR = LevelPTR;
        }

        public void LoadContent(ContentManager Content)
        {
            MapBG = Content.Load<Texture2D>(@"Textures\IngameMenu\Map\Map");
            MinimapSquare = Content.Load<Texture2D>(@"Textures\IngameMenu\Map\MinimapSquare");

            SetPositions();
        }

        public override void Open()
        {
            PanOffset = Vector2.Zero;
            Main.LastGamePadState = Main.GamePadState;
        }

        private void SetPositions()
        {
            MapDrawPos = new Vector2(Main.BackBufferWidth / 2 - MapBG.Width / 2,
                    Main.BackBufferHeight / 2 - MapBG.Height / 2);
        }

        float XPanDelay = 0f;
        float YPanDelay = 0f;

        public override void Update(GameTime GameTime)
        {
            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Back) &&
                !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Back))
            {
                MenuManager.CloseMenu(this);
            }

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Start) &&
               !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Start))
            {
                MenuManager.CloseMenu(this);
            }

            XPanDelay -= GameTime.ElapsedGameTime.Milliseconds;
            YPanDelay -= GameTime.ElapsedGameTime.Milliseconds;

            if (XPanDelay < 0f)
                XPanDelay = 0f;
            if (YPanDelay < 0f)
                YPanDelay = 0f;

            //Pan U/D
            if (YPanDelay == 0f)
            {
                if (Main.KeyboardState.IsKeyDown(Keys.Up) || Main.GamePadState.ThumbSticks.Left.Y > 0.35f)
                {
                    PanOffset.Y -= 2;
                    YPanDelay = 100f;
                }
                else if (Main.KeyboardState.IsKeyDown(Keys.Down) || Main.GamePadState.ThumbSticks.Left.Y < -0.35f)
                {
                    PanOffset.Y += 2;
                    YPanDelay = 100f;
                }
            }
            //Pan L/R
            if (XPanDelay == 0f)
            {
                if (Main.KeyboardState.IsKeyDown(Keys.Left) || Main.GamePadState.ThumbSticks.Left.X < -0.35f)
                {
                    PanOffset.X--;

                    XPanDelay = 100f;
                }
                else if (Main.KeyboardState.IsKeyDown(Keys.Right) || Main.GamePadState.ThumbSticks.Left.X > 0.35f)
                {

                    PanOffset.X++;

                    XPanDelay = 100f;
                }
            }

        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            //Draw if visible

            SpriteBatch.Begin();

            SpriteBatch.Draw(MapBG, MapDrawPos, Color.White);

            //Mess with (Playerx - xxxxx) to change where it starts drawing
            Vector2 FirstSquare = new Vector2(
                (LevelPTR.GetPlayerX() - Main.BackBufferWidth) / Tile.TileStepX,
                (LevelPTR.GetPlayerY() - Main.BackBufferHeight) / Tile.TileStepY);

            int FirstX = (int)MathHelper.Clamp(FirstSquare.X - 1 + (int)PanOffset.X, 0, LevelPTR.LevelMap.MapWidth);
            int FirstY = (int)MathHelper.Clamp(FirstSquare.Y - 3 + (int)PanOffset.Y, 0, LevelPTR.LevelMap.MapHeight);

            Vector2 LastSquare = new Vector2(
                 (LevelPTR.GetPlayerX() + Main.BackBufferWidth) / Tile.TileStepX,
                (LevelPTR.GetPlayerY() + Main.BackBufferHeight) / Tile.TileStepY);

            int LastX = (int)MathHelper.Clamp(LastSquare.X + 1 + (int)PanOffset.X, 0, LevelPTR.LevelMap.MapWidth);
            int LastY = (int)MathHelper.Clamp(LastSquare.Y + (int)PanOffset.Y, 0, LevelPTR.LevelMap.MapHeight);

            for (int Y = 0; Y < LastY; Y++)
            {
                //Determine if there is a row offset if it is an odd row
                int RowOffset = 0;
                if ((FirstY + Y) % 2 == 1)
                    RowOffset = MinimapSquare.Width / 2;

                for (int X = 0; X < LastX; X++)
                {
                    //Get the offset and keep it within bounds
                    int MapX = FirstX + X;
                    int MapY = FirstY + Y;

                    if ((MapX < 0) || (MapY < 0) ||
                        (MapX >= LastX) || (MapY >= LastY) ||
                         (!LevelPTR.LevelMap.MapCells[MapY, MapX].Walkable) ||
                        LevelPTR.LevelMap.MapCells[MapY, MapX].TileID == -1)
                        continue;

                    //1 - DRAW BASE TILE

                    if (MapX == LevelPTR.LevelMap.WorldToMapCell(new Point((int)LevelPTR.GetPlayerX(), (int)LevelPTR.GetPlayerY())).X

                        &&

                        (MapY == LevelPTR.LevelMap.WorldToMapCell(new Point((int)LevelPTR.GetPlayerX(), (int)LevelPTR.GetPlayerY())).Y))
                    {
                        SpriteBatch.Draw(MinimapSquare,
                            new Vector2(X * MinimapSquare.Width + (Main.BackBufferWidth / 2 - MapBG.Width / 2) + RowOffset + 200,
                                Y * MinimapSquare.Height + (Main.BackBufferHeight / 2 - MapBG.Height / 2) + 120),
                            Color.Red);
                    }
                    else
                    {
                        SpriteBatch.Draw(MinimapSquare,
                            new Vector2(X * MinimapSquare.Width + (Main.BackBufferWidth / 2 - MapBG.Width / 2) + RowOffset + 200,
                                Y * MinimapSquare.Height + (Main.BackBufferHeight / 2 - MapBG.Height / 2) + 120),
                            Color.Black);
                    }
                }
            }

            SpriteBatch.End();
        }


    }
}
