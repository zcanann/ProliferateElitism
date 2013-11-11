using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace River
{
    class LootMenu : Menu
    {
        private Vector2 SelectionPos;
        private Vector2 CurrentIndex;
        private Level LevelPTR;

        public LootMenu(MenuSideType MenuSideType, Level LevelPTR)
            : base(MenuSideType)
        {
            this.LevelPTR = LevelPTR;
        }

        public override void Open()
        {
            CurrentIndex = Vector2.Zero;
        }

        private void ScrollVertical(int Amount)
        {
            CurrentIndex.Y += Amount;

            //Bounds checking
            if (CurrentIndex.Y < 0)
                CurrentIndex.Y = 0;
            if (CurrentIndex.Y >= LootInventory.LootSlots)
                CurrentIndex.Y = LootInventory.LootSlots - 1;
        }

        private void ScrollHorizontal(int Amount)
        {
            CurrentIndex.X += Amount;
            if (CurrentIndex.X >= 1)
            {
                CurrentIndex.X = 0;
                SelectionDelayX = Main.StandardDelay;
                SelectionDelayY = Main.StandardDelay;
                MenuManager.ChangeFocus();
            }
        }

        private Vector2 GetSelectionDrawPos()
        {
            return new Vector2(LevelPTR.Player.LootingInventory.BasePosition.X,
                CurrentIndex.Y * LootInventory.LootStepSize + LevelPTR.Player.LootingInventory.BasePosition.Y);
        }

        private int GetSelectorIndex()
        {

            return (int)CurrentIndex.Y;
        }

        private float SelectionDelayY = 0f;
        private float SelectionDelayX = 0f;
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
                //Looting causes a double push (ie click-once looting) only if there are no inv. selections
                if (!SwapHelper.HasSelection())
                    SwapHelper.Push(LevelPTR.Player.LootingInventory, GetSelectorIndex());
                SwapHelper.Push(LevelPTR.Player.LootingInventory, GetSelectorIndex());
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

            //Right
            if (Main.GamePadState.ThumbSticks.Left.X > 0.5f &&
                SelectionDelayX == 0f)
            {
                SelectionDelayX = Main.StandardDelay;
                ScrollHorizontal(1);
            }

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

            LevelPTR.Player.LootingInventory.Draw(SpriteBatch);

            //Draw selections
            if (MenuManager.HasFocus(this))
            {
                SpriteBatch.Draw(SwapHelper.IconSelector, GetSelectionDrawPos(), Color.White);

                if (LevelPTR.Player.LootingInventory.Items[GetSelectorIndex()] != Item.None)
                    LevelPTR.Player.LootingInventory.Items[GetSelectorIndex()].DrawText(SpriteBatch, GetSelectionDrawPos(), Item.PriceDisplayType.Sell);
            }

            if (SwapHelper.HasSelection())
                if (SwapHelper.GetSelectionInventory() == LevelPTR.Player.LootingInventory)
                    SpriteBatch.Draw(SwapHelper.IconSelected, SelectionPos, Color.White);

            SpriteBatch.End();
        }

    }
}
