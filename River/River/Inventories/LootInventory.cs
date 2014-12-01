using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace River
{
    class LootInventory : StandardInventory
    {
        public const int LootStepSize = Item.IconSize;
        //public const int EnemyRowSize = 1;
        public const int LootSlots = 5;
        protected static Random Random = new Random();

        public bool GenerateOnLoot = false;

        

        public LootInventory(Int32 MyInventoryID)
            : base(MyInventoryID)
        {
            //Base will just call our initialize
        }

        protected override void Initialize()
        {
            this.Items = new Item[LootSlots];

            BasePosition.X = (Main.BackBufferWidth / 2 - Item.IconSize) / 2;
            BasePosition.Y = Item.IconSize;

            TextDrawPosition.X = BasePosition.X + Item.IconSize * 1;
            TextDrawPosition.Y = BasePosition.Y + (InventorySlots / InventoryRowSize) * Item.IconSize + Item.IconSize;
        }

        public virtual void GenerateDrops(int BaseLevel, int AmountGuarenteed = 0)
        {
            bool Guarenteed = false;
            int NextIndex = 0;

            for (int ecx = 0; ecx < this.Items.Length; ecx++)
            {
                if (ecx < AmountGuarenteed)
                    Guarenteed = true;
                else
                    Guarenteed = false;

                this.Items[NextIndex] = GetRandomItem(BaseLevel, Guarenteed);
                if (this.Items[ecx] != Item.None)
                    NextIndex++;
            }

        }

        protected virtual Item GetRandomItem(int BaseLevel, bool IsGuarenteed)
        {

            if (!IsGuarenteed && Random.NextDouble() > 0.33)
                return Item.None;
            
            return GameDB.AddRandomItem(InventoryID, BaseLevel);
        }

        //Just draw in a vertical line down
        public override void Draw(SpriteBatch SpriteBatch)
        {
            Vector2 DrawPosition = new Vector2(BasePosition.X, BasePosition.Y);

            for (int ecx = 0; ecx < Items.Length; ecx++)
            {
                if (Items[ecx] != Item.None)
                    Items[ecx].Draw(SpriteBatch, DrawPosition);
                else
                    SpriteBatch.Draw(Item.IconFrame[0], DrawPosition, Color.White);

                DrawPosition.Y += Item.IconSize;
            }
        }

    } //End draw

}
