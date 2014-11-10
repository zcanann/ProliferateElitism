using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace River
{
    class StandardInventory
    {
        public Item[] Items;
        public Vector2 BasePosition = new Vector2();
        protected Vector2 TextDrawPosition = new Vector2();
        public const int InventoryStepSize = Item.IconSize;
        public const int InventoryRowSize = 8;
        public const int InventorySlots = 40;


        public StandardInventory()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            Items = new Item[InventorySlots];

            //for (int ecx = 0; ecx < InventorySlots; ecx++)
            //    Items[ecx] = new Items.Junk(0, 0);

            //Create shoes/boots for debugging
            //for (int ecx = 0; ecx < InventoryRowSize * 2; ecx++)
            //    Items[ecx] = new Items.Feet(0, Player.MaxMagicFind);

            BasePosition.X = Main.BackBufferWidth / 2 + (Main.BackBufferWidth / 2 - InventoryRowSize * Item.IconSize) / 2;
            BasePosition.Y = Item.IconSize;

            TextDrawPosition.X = BasePosition.X + Item.IconSize * 3;
            TextDrawPosition.Y = BasePosition.Y + (InventorySlots / InventoryRowSize) * Item.IconSize + Item.IconSize;
        }

        public bool IsEmpty()
        {
            //Check for non empty items
            for (int ecx = 0; ecx < Items.Length; ecx++)
                if (Items[ecx] != Item.None)
                    return false;
            //All items were empty
            return true;
        }

        public static void SwapItems(StandardInventory Source, int SourceIndex, StandardInventory Destination, int DestinationIndex)
        {
            //Grab first item and store in Temp
            Item Temp = Source.Items[SourceIndex];

            //Swap
            Source.Items[SourceIndex] = Destination.Items[DestinationIndex];
            Destination.Items[DestinationIndex] = Temp;
        }

        public virtual void Draw(SpriteBatch SpriteBatch)
        {
            Vector2 DrawPosition = new Vector2(BasePosition.X, BasePosition.Y);

            for (int ecx = 0; ecx < Items.Length; ecx++)
            {
                if (Items[ecx] != Item.None)
                    Items[ecx].Draw(SpriteBatch, DrawPosition);
                else
                    SpriteBatch.Draw(Item.IconFrame[0], DrawPosition, Color.White);

                DrawPosition.X += InventoryStepSize;

                if ((ecx + 1) % InventoryRowSize == 0)
                {
                    DrawPosition.Y += InventoryStepSize;
                    DrawPosition.X = BasePosition.X;
                }
            }

        } //End draw


    }
}
