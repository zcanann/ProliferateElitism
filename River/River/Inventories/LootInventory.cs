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

        public LootInventory()
            : base()
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

        public virtual void GenerateDrops(EntityType EntityType, int EnemyLevel, float MagicFind, bool IsBoss)
        {
            if (MagicFind > Player.MaxMagicFind)
                MagicFind = Player.MaxMagicFind;

            //this.Items[0] = GetJunkItem();

            int NextIndex = 0;
            for (int ecx = 0; ecx < this.Items.Length; ecx++)
            {
                this.Items[NextIndex] = GetRandomItem(EntityType, EnemyLevel, MagicFind, IsBoss);
                if (this.Items[ecx] != Item.None)
                    NextIndex++;
            }

        }

        protected Item GetJunkItem(EntityType EntityType, int EnemyLevel, float MagicFind)
        {
            int Chance = Random.Next(0, 101); //0 to 100

            return new Items.Gem(EnemyLevel, MagicFind, EntityType);

        }

        protected virtual Item GetRandomItem(EntityType EntityType, int EnemyLevel, float MagicFind, bool IsBoss)
        {
            int Chance;

            if (IsBoss)
                Chance = 100;
            else
                Chance = Random.Next(0, 101); //0 to 100

            if (Chance > 80) //20% chance per item (unless boss)
            {
                switch (Random.Next(0, Enum.GetValues(typeof(Item.SlotType)).Length - 1))
                {
                    case (int)Item.SlotType.Amulet:
                        return new Items.Amulet(EnemyLevel, MagicFind);
                    case (int)Item.SlotType.Chest:
                        return new Items.Chest(EnemyLevel, MagicFind);
                    case (int)Item.SlotType.Feet:
                        return new Items.Feet(EnemyLevel, MagicFind);
                    case (int)Item.SlotType.Hands:
                        return new Items.Hands(EnemyLevel, MagicFind);
                    case (int)Item.SlotType.Head:
                        return new Items.Head(EnemyLevel, MagicFind);
                    case (int)Item.SlotType.Legs:
                        return new Items.Legs(EnemyLevel, MagicFind);
                    case (int)Item.SlotType.Offhand:
                        return new Items.Offhand(EnemyLevel, MagicFind);
                    case (int)Item.SlotType.Ring:
                        return new Items.Ring(EnemyLevel, MagicFind);
                    case (int)Item.SlotType.Weapon:
                        return new Items.Weapon(EnemyLevel, MagicFind);
                }
            }

            return null;
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
