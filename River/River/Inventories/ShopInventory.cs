using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace River
{
    class ShopInventory : StandardInventory
    {
        public const int ShopStepSize = Item.IconSize;
        public const int ShopRowSize = 8;
        public const int ShopSlots = 56;
        private static Random Random = new Random();

        private static Level LevelPTR;

        public ShopInventory(Level _LevelPTR, Int32 InventoryID) :
            base(InventoryID)
        {
            LevelPTR = _LevelPTR;
        }

        protected override void Initialize()
        {
            Items = new Item[ShopSlots];

            BasePosition.X = (Main.BackBufferWidth / 2 - ShopRowSize * Item.IconSize) / 2;
            BasePosition.Y = Item.IconSize;

            TextDrawPosition.X = BasePosition.X + Item.IconSize * 3;
            TextDrawPosition.Y = BasePosition.Y + (ShopSlots / ShopRowSize) * Item.IconSize + Item.IconSize;
        }

        new public static void SwapItems(StandardInventory Source, int SourceIndex, StandardInventory Destination, int DestinationIndex)
        {
            long GoldChange = 0;

            if (Destination.Items[DestinationIndex] != Item.None)
                if (Destination == LevelPTR.Player.Inventory)
                    GoldChange = Destination.Items[DestinationIndex].SellPrice;
                else
                    GoldChange = Destination.Items[DestinationIndex].BuyPrice;

            if (Source.Items[SourceIndex] != Item.None)
                GoldChange -= Source.Items[SourceIndex].BuyPrice;

            //Same inventory = no gold change
            if (Source == Destination)
                GoldChange = 0;

            if (LevelPTR.Player.Gold + GoldChange >= 0)
            {
                LevelPTR.Player.Gold += GoldChange;
                //Grab first item and store in Temp
                Item Temp = Source.Items[SourceIndex];
                //Swap
                Source.Items[SourceIndex] = Destination.Items[DestinationIndex];
                Destination.Items[DestinationIndex] = Temp;
            }

        }

        public void GenerateShopItems(int PlayerLevel)
        {
            //Fill first row with blue items
            //for (int ecx = 0; ecx < ShopRowSize * 4; ecx++)
            //    this.Items[ecx] = GetRandomBlueItem(PlayerLevel);
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            Vector2 DrawPosition = new Vector2(BasePosition.X, BasePosition.Y);

            for (int ecx = 0; ecx < Items.Length; ecx++)
            {
                if (Items[ecx] != Item.None)
                    Items[ecx].Draw(SpriteBatch, DrawPosition);
                else
                    SpriteBatch.Draw(Item.IconFrame[0], DrawPosition, Color.White);

                DrawPosition.X += ShopStepSize;

                if ((ecx + 1) % ShopRowSize == 0)
                {
                    DrawPosition.Y += ShopStepSize;
                    DrawPosition.X = BasePosition.X;
                }
            }

        } //End draw



    }
}
