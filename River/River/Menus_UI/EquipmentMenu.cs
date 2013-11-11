using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace River
{
    class EquipmentMenu : Menu
    {
        private Vector2 SelectionPos;
        private Vector2 CurrentIndex;
        private Level LevelPTR;

        public EquipmentMenu(MenuSideType MenuSideType, Level LevelPTR)
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
            if (CurrentIndex.X >= Equipment.EquipmentRowSize)
            {
                CurrentIndex.X = Equipment.EquipmentRowSize - 1;
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
            if (CurrentIndex.Y >= Equipment.EquipmentSlots / Equipment.EquipmentRowSize)
                CurrentIndex.Y = Equipment.EquipmentSlots / Equipment.EquipmentRowSize - 1;

        }

        private Vector2 GetSelectionDrawPos()
        {
            return new Vector2(CurrentIndex.X * Equipment.EquipmentStepSize + LevelPTR.Player.Equipment.BasePosition.X,
                CurrentIndex.Y * Equipment.EquipmentStepSize + LevelPTR.Player.Equipment.BasePosition.Y);
        }

        private int GetSelectorIndex()
        {
            return (int)(CurrentIndex.Y * Equipment.EquipmentRowSize + CurrentIndex.X);
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
                SwapHelper.Push(LevelPTR.Player.Equipment, GetSelectorIndex());
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
            if (Main.GamePadState.ThumbSticks.Left.X > 0.5f &&
                SelectionDelayX == 0f)
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

            LevelPTR.Player.Equipment.Draw(SpriteBatch);

            DrawStats(SpriteBatch);

            //Draw selections
            if (MenuManager.HasFocus(this))
            {
                SpriteBatch.Draw(SwapHelper.IconSelector, GetSelectionDrawPos(), Color.White);
                if (LevelPTR.Player.Equipment.Items[GetSelectorIndex()] != Item.None)
                    LevelPTR.Player.Equipment.Items[GetSelectorIndex()].DrawText(SpriteBatch, GetSelectionDrawPos(), Item.PriceDisplayType.Sell);
            }
            if (SwapHelper.HasSelection())
                if (SwapHelper.GetSelectionInventory() == LevelPTR.Player.Equipment)
                    SpriteBatch.Draw(SwapHelper.IconSelected, SelectionPos, Color.White);

            

            SpriteBatch.End();
        }

        private Vector2 DrawPos = new Vector2();
        private void DrawStats(SpriteBatch SpriteBatch)
        {
            DrawPos.X = Item.IconSize * 2;
            DrawPos.Y = Item.IconSize * 6;

            SpriteBatch.Draw(Item.StatPanel, new Vector2(DrawPos.X - Item.IconSize / 4, DrawPos.Y - Item.IconSize / 4), Color.White);

            for (int ecx = 0; ecx < (LevelPTR.Player.StatText.Length + 1) / 2; ecx++)
            {
                SpriteBatch.DrawString(Item.ItemTextFont, LevelPTR.Player.StatText[ecx], DrawPos, Color.White);
                DrawPos.Y += 32;
            }
            DrawPos.X = Item.IconSize * 5;
            DrawPos.Y = Item.IconSize * 6;
            for (int ecx = (LevelPTR.Player.StatText.Length + 1) / 2; ecx < LevelPTR.Player.StatText.Length; ecx++)
            {
                SpriteBatch.DrawString(Item.ItemTextFont, LevelPTR.Player.StatText[ecx], DrawPos, Color.White);
                DrawPos.Y += 32;
            }
        }

    }
}
