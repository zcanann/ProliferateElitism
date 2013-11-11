using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace River
{
    class StorageMenu : Menu
    {
        private Vector2 SelectionPos;
        private Vector2 CurrentIndex;
        private Level LevelPTR;

        public StorageMenu(MenuSideType MenuSideType, Level LevelPTR)
            : base(MenuSideType)
        {
            this.LevelPTR = LevelPTR;
        }

        public override void Open()
        {
            CurrentIndex = Vector2.Zero;
        }

        private void ScrollHorizontal(int Amount)
        {
            CurrentIndex.X += Amount;

            //Bounds checking
            if (CurrentIndex.X < 0)
                CurrentIndex.X = 0;

            if (CurrentIndex.X >= StorageInventory.StorageRowSize)
            {
                CurrentIndex.X = StorageInventory.StorageRowSize - 1;
                SelectionDelayX = Main.StandardDelay;
                SelectionDelayY = Main.StandardDelay;
                MenuManager.ChangeFocus();
            }
        }

        private void ScrollVertical(int Amount)
        {
            CurrentIndex.Y += Amount;

            //Bounds checking
            if (CurrentIndex.Y < 0)
                CurrentIndex.Y = 0;
            if (CurrentIndex.Y >= StorageInventory.StorageSlots / StorageInventory.StorageRowSize)
                CurrentIndex.Y = StorageInventory.StorageSlots / StorageInventory.StorageRowSize - 1;
        }

        private Vector2 GetSelectionDrawPos()
        {
            return new Vector2(CurrentIndex.X * StorageInventory.StorageStepSize + LevelPTR.StorageInventory.BasePosition.X,
                    CurrentIndex.Y * StorageInventory.StorageStepSize + LevelPTR.StorageInventory.BasePosition.Y);
        }

        private int GetSelectorIndex()
        {
            return (int)(CurrentIndex.Y * StorageInventory.StorageRowSize + CurrentIndex.X);
        }

        private float SelectionDelayX = 0f;
        private float SelectionDelayY = 0f;
        public override void Update(GameTime GameTime)
        {

            SelectionDelayX -= GameTime.ElapsedGameTime.Milliseconds;
            SelectionDelayY -= GameTime.ElapsedGameTime.Milliseconds;

            if (SelectionDelayX < 0f)
                SelectionDelayX = 0f;

            if (SelectionDelayY < 0f)
                SelectionDelayY = 0f;

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) &&
               !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
            {
                SwapHelper.Push(LevelPTR.StorageInventory, GetSelectorIndex());
                SelectionPos = GetSelectionDrawPos();
            }

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.B) &&
               !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.B))
            {
                if (SwapHelper.HasSelection())
                    SwapHelper.Reset();
                else
                {
                    SwapHelper.Disconnect();
                    MenuManager.CloseAll();
                }
            }

            //Left
            if (Main.GamePadState.ThumbSticks.Left.X < -0.5f &&
                SelectionDelayX == 0f)
            {
                SelectionDelayX = Main.StandardDelay;
                ScrollHorizontal(-1);
            }

            //Right
            if (Main.GamePadState.ThumbSticks.Left.X > 0.5f && SelectionDelayX == 0f)
            {
                SelectionDelayX = Main.StandardDelay;
                ScrollHorizontal(1);
            }

            if (Main.GamePadState.Triggers.Right > 0.5f)
                ScrollHorizontal(999);

            //Down
            if (Main.GamePadState.ThumbSticks.Left.Y < -0.5f &&
                SelectionDelayY == 0f)
            {
                SelectionDelayY = Main.StandardDelay;
                ScrollVertical(1);
            }

            //Up
            if (Main.GamePadState.ThumbSticks.Left.Y > 0.5f &&
                SelectionDelayY == 0f)
            {
                SelectionDelayY = Main.StandardDelay;
                ScrollVertical(-1);
            }
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {

            SpriteBatch.Begin();

            LevelPTR.StorageInventory.Draw(SpriteBatch);

            //Draw selections
            if (MenuManager.HasFocus(this))
            {
                SpriteBatch.Draw(SwapHelper.IconSelector, GetSelectionDrawPos(), Color.White);

                //Draw item stats
                if (LevelPTR.StorageInventory.Items[GetSelectorIndex()] != Item.None)
                    LevelPTR.StorageInventory.Items[GetSelectorIndex()].DrawText(SpriteBatch, GetSelectionDrawPos(), Item.PriceDisplayType.Sell);
            }

            if (SwapHelper.HasSelection())
                if (SwapHelper.GetSelectionInventory() == LevelPTR.StorageInventory)
                    SpriteBatch.Draw(SwapHelper.IconSelected, SelectionPos, Color.White);

            SpriteBatch.End();
        }

    }
}
