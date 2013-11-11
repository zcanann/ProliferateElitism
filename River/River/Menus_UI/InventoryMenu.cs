using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace River
{
    class InventoryMenu : Menu
    {
        private Vector2 SelectionPos;
        private Vector2 CurrentIndex;
        private Level LevelPTR;

        private Vector2 GoldPanelPos;
        private Vector2 GoldTextPos;

        public InventoryMenu(MenuSideType MenuSideType, Level LevelPTR)
            : base(MenuSideType)
        {
            this.LevelPTR = LevelPTR;
        }

        public override void Open()
        {
            CurrentIndex = Vector2.Zero;
            GoldPanelPos = new Vector2(LevelPTR.Player.Inventory.BasePosition.X,
               LevelPTR.Player.Inventory.BasePosition.Y +
               (StandardInventory.InventorySlots / StandardInventory.InventoryRowSize) * StandardInventory.InventoryStepSize);
            GoldTextPos = new Vector2(GoldPanelPos.X + 16, GoldPanelPos.Y + 12);
        }

        private void ScrollHorizontal(int Amount)
        {
            CurrentIndex.X += Amount;

            //Bounds checking
            if (CurrentIndex.X < 0)
            {
                CurrentIndex.X = 0;
                SelectionDelayX = Main.StandardDelay;
                SelectionDelayY = Main.StandardDelay;
                MenuManager.ChangeFocus();
            }
            if (CurrentIndex.X >= StandardInventory.InventoryRowSize)
                CurrentIndex.X = StandardInventory.InventoryRowSize - 1;
        }

        private void ScrollVertical(int Amount)
        {
            CurrentIndex.Y += Amount;

            //Bounds checking
            if (CurrentIndex.Y < 0)
                CurrentIndex.Y = 0;
            if (CurrentIndex.Y >= StandardInventory.InventorySlots / StandardInventory.InventoryRowSize)
                CurrentIndex.Y = StandardInventory.InventorySlots / StandardInventory.InventoryRowSize - 1;
        }

        private Vector2 GetSelectionDrawPos()
        {
            return new Vector2(CurrentIndex.X * StandardInventory.InventoryStepSize + LevelPTR.Player.Inventory.BasePosition.X,
                    CurrentIndex.Y * StandardInventory.InventoryStepSize + LevelPTR.Player.Inventory.BasePosition.Y);
        }

        private int GetSelectorIndex()
        {
            return (int)(CurrentIndex.Y * StandardInventory.InventoryRowSize + CurrentIndex.X);
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
                SwapHelper.Push(LevelPTR.Player.Inventory, GetSelectorIndex());
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
            if (Main.GamePadState.Triggers.Left > 0.5f)
                ScrollHorizontal(-999);

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

            LevelPTR.Player.Inventory.Draw(SpriteBatch);

            //Draw gold
            SpriteBatch.Draw(Item.GoldPanel, GoldPanelPos, Color.White);
            SpriteBatch.DrawString(Item.ItemTextFont, "Gold: " + LevelPTR.Player.Gold.ToString("N0"), GoldTextPos, Color.White);

            //Draw selections
            if (MenuManager.HasFocus(this))
            {
                SpriteBatch.Draw(SwapHelper.IconSelector, GetSelectionDrawPos(), Color.White);

                if (LevelPTR.Player.Inventory.Items[GetSelectorIndex()] != Item.None)
                    LevelPTR.Player.Inventory.Items[GetSelectorIndex()].DrawText(SpriteBatch, GetSelectionDrawPos(), Item.PriceDisplayType.Sell);
            }

            if (SwapHelper.HasSelection())
                if (SwapHelper.GetSelectionInventory() == LevelPTR.Player.Inventory)
                    SpriteBatch.Draw(SwapHelper.IconSelected, SelectionPos, Color.White);

            SpriteBatch.End();
        }
    }
}
