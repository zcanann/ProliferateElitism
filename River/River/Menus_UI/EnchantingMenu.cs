using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace River
{
    class EnchantingMenu : Menu
    {
        private Level LevelPTR;

        public EnchantingMenu(MenuSideType MenuSideType, Level LevelPTR)
            : base(MenuSideType)
        {
            this.LevelPTR = LevelPTR;
        }

        private void RetrieveEnchantItem()
        {
            SwapHelper.Push(LevelPTR.EnchantingInventory, 0);
            SwapHelper.Push(LevelPTR.EnchantingInventory, 0);
        }

        public override void Open()
        {
            SwapHelper.SetDisconnectCallBack(new SwapHelper.SwapDisconnect(RetrieveEnchantItem));
        }

        private void ScrollHorizontal()
        {
            SelectionDelayX = Main.StandardDelay;
            MenuManager.ChangeFocus();
        }

        private float SelectionDelayX = 0f;
        public override void Update(GameTime GameTime)
        {
            SelectionDelayX -= GameTime.ElapsedGameTime.Milliseconds;
            if (SelectionDelayX < 0f)
                SelectionDelayX = 0f;

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) &&
               !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
            {
                SwapHelper.Push(LevelPTR.EnchantingInventory, 0);
            }

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Y) &&
             !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Y))
            {
                if (LevelPTR.EnchantingInventory.Items[0] != Item.None)
                    if (LevelPTR.EnchantingInventory.Items[0].CanEnchant())
                        if (LevelPTR.Player.Gold >= LevelPTR.EnchantingInventory.Items[0].EnchantPrice)
                        {
                            LevelPTR.Player.Gold -= LevelPTR.EnchantingInventory.Items[0].EnchantPrice;
                            //Try to do enchant
                            if (!LevelPTR.EnchantingInventory.Items[0].DoEnchant())
                                //Failed; kill the item
                                LevelPTR.EnchantingInventory.Items[0] = Item.None;

                            //TODO: SAVE GAME HERE
                        }
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
                ScrollHorizontal();
            }

            if (Main.GamePadState.Triggers.Right > 0.5f)
                ScrollHorizontal();

        }

        public override void Draw(SpriteBatch SpriteBatch)
        {

            SpriteBatch.Begin();

            LevelPTR.EnchantingInventory.Draw(SpriteBatch);

            //Draw selections
            if (MenuManager.HasFocus(this))
            {
                SpriteBatch.Draw(SwapHelper.IconSelector, LevelPTR.EnchantingInventory.BasePosition, Color.White);

                if (LevelPTR.EnchantingInventory.Items[0] != Item.None)
                    LevelPTR.EnchantingInventory.Items[0].DrawText(SpriteBatch, LevelPTR.EnchantingInventory.BasePosition, Item.PriceDisplayType.Enchant);
            }

            if (SwapHelper.HasSelection())
                if (SwapHelper.GetSelectionInventory() == LevelPTR.EnchantingInventory)
                    SpriteBatch.Draw(SwapHelper.IconSelected, LevelPTR.EnchantingInventory.BasePosition, Color.White);

            SpriteBatch.End();
        }

    }
}
