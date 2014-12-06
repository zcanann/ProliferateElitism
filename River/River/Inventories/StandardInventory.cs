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

        protected Int32 InventoryID = -1;


        public StandardInventory(Int32 InventoryID)
        {
            this.InventoryID = InventoryID;

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

        public virtual void LoadItems()
        {
            List<Object> LoadItems = GameDB.GetItemsFromInventory(InventoryID.ToString());

            for (int ecx = 0; ecx < LoadItems.Count; ecx++)
            {
                this.Items[ecx] = GameDB.ReadItemFromDataBase((Int32)LoadItems[ecx]);
            }
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
            // Update item ownership in the database
            UpdateOwnership(Source, SourceIndex, Destination, DestinationIndex);

            //Grab first item and store in Temp
            Item Temp = Source.Items[SourceIndex];

            //Swap
            Source.Items[SourceIndex] = Destination.Items[DestinationIndex];
            Destination.Items[DestinationIndex] = Temp;
        }

        protected static void UpdateOwnership(StandardInventory Source, int SourceIndex, StandardInventory Destination, int DestinationIndex)
        {
            // Swapping nothing with nothing, who cares.
            if (Source.Items[SourceIndex] == Item.None && Destination.Items[DestinationIndex] == Item.None)
                return;

            if (Source.Items[SourceIndex] != Item.None)
            {
                GameDB.UpdateOwnerShip(Source.InventoryID, Destination.InventoryID, Source.Items[SourceIndex].ItemID);
            }
            if (Destination.Items[DestinationIndex] != Item.None)
            {
                GameDB.UpdateOwnerShip(Destination.InventoryID, Source.InventoryID, Destination.Items[DestinationIndex].ItemID);
            }
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
