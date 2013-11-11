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
    class LevelEditor
    {
        private Level LevelPTR;
        private Texture2D HiLight;
        private RoomEditor RoomEditor;

        public LevelEditor(Level LevelPTR)
        {
            this.LevelPTR = LevelPTR;
            RoomEditor = new RoomEditor(LevelPTR);
        }

        public void LoadContent(ContentManager Content)
        {
            HiLight = Content.Load<Texture2D>(@"Textures\Tiles\HiLight");
        }

        private MouseState MouseState;
        private MouseState LastMouseState;
        private Vector2 MoveDir;
        private MapCell CopyMe;
        private Point SourcePoint;
        public void Update(GameTime GameTime)
        {
            MouseState = Mouse.GetState();

            if (MouseState.LeftButton == ButtonState.Pressed || MouseState.RightButton == ButtonState.Pressed)
            {
                //Load data if just a single left click
                Vector2 HiLightLoc = Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                Point HiLightPoint = LevelPTR.LevelMap.WorldToMapCell(new Point((int)HiLightLoc.X, (int)HiLightLoc.Y));

                //Offscreen click check
                if (HiLightLoc.X < Camera.Location.X ||
                    HiLightLoc.X > Camera.Location.X + Main.BackBufferWidth ||
                    HiLightLoc.Y < Camera.Location.Y ||
                    HiLightLoc.Y > Camera.Location.Y + Main.BackBufferHeight)
                    goto Escape;

                RoomEditor.LoadTileData(LevelPTR.LevelMap.GetTileData(HiLightPoint.X, HiLightPoint.Y), HiLightPoint);

                if (MouseState.RightButton == ButtonState.Pressed)
                {
                    if (LastMouseState.RightButton != ButtonState.Pressed)
                    {
                        SourcePoint = HiLightPoint;
                        CopyMe = LevelPTR.LevelMap.GetTileData(HiLightPoint.X, HiLightPoint.Y);
                    }
                    else
                    {
                        if (CopyMe != null)
                        {
                            if (HiLightPoint != SourcePoint)
                                LevelPTR.LevelMap.ChangeTileData(CopyMe, HiLightPoint);
                        }

                    }

                    if (MouseState.MiddleButton == ButtonState.Pressed)
                    {
                        Fill(HiLightPoint, true);
                    }

                }
            }

        Escape:

            LastMouseState = MouseState;

            MoveDir.X = Main.GamePadState.ThumbSticks.Left.X * 2;
            MoveDir.Y = -Main.GamePadState.ThumbSticks.Left.Y;
            if (Main.KeyboardState.IsKeyDown(Keys.Up))
                MoveDir.Y = -1;
            else if (Main.KeyboardState.IsKeyDown(Keys.Down))
                MoveDir.Y = 1;
            if (Main.KeyboardState.IsKeyDown(Keys.Left))
                MoveDir.X = -2;
            else if (Main.KeyboardState.IsKeyDown(Keys.Right))
                MoveDir.X = 2;
            Vector2.Multiply(ref MoveDir, 3f * GameTime.ElapsedGameTime.Milliseconds / Main.SpeedConst, out MoveDir);
            Camera.Move(MoveDir);
        }

        private void Fill(Point FillPt, bool First)
        {

            if (!LevelPTR.LevelMap.IsValidSquare(FillPt))
                return;

            if (!First && LevelPTR.LevelMap.GetTileData(FillPt.X, FillPt.Y).TileID != -1)
                return;

            LevelPTR.LevelMap.ChangeTileData(CopyMe, FillPt);

            if (FillPt.Y % 2 == 0)
            {
                Fill(new Point(FillPt.X - 1, FillPt.Y - 1), false);
                Fill(new Point(FillPt.X, FillPt.Y - 1), false);
                Fill(new Point(FillPt.X, FillPt.Y + 1), false);
                Fill(new Point(FillPt.X - 1, FillPt.Y + 1), false);
            }
            else
            {
                Fill(new Point(FillPt.X, FillPt.Y - 1), false);
                Fill(new Point(FillPt.X + 1, FillPt.Y - 1), false);
                Fill(new Point(FillPt.X, FillPt.Y + 1), false);
                Fill(new Point(FillPt.X + 1, FillPt.Y + 1), false);
            }

        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            //HIGHLIGHT SELECTED TILE
            Vector2 HiLightLoc = Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            Point HiLightPoint = LevelPTR.LevelMap.WorldToMapCell(new Point((int)HiLightLoc.X, (int)HiLightLoc.Y));

            int HiLightRowOffset = 0;
            if ((HiLightPoint.Y) % 2 == 1)
                HiLightRowOffset = Tile.OddRowXOffset;

            SpriteBatch.Begin();

            SpriteBatch.Draw(HiLight,
                            Camera.WorldToScreen(
                                new Vector2(
                                    HiLightPoint.X * Tile.TileStepX + HiLightRowOffset,
                                    HiLightPoint.Y * Tile.TileStepY)),
                            HiLight.Bounds,
                            Color.White * 0.3f,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);

            SpriteBatch.End();
        }

    }
}
