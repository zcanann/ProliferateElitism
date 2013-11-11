using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River
{
    class ChestInventory : LootInventory
    {
        public ChestInventory()
            : base()
        {
            GenerateOnLoot = true;
        }

        private bool Generated = false;

        public bool IsGenerated()
        {
            return Generated;
        }

        public override void GenerateDrops(EntityType EntityType, int EnemyLevel, float MagicFind, bool IsGuarenteed)
        {
            Generated = true;
            GenerateOnLoot = false;

            if (MagicFind > Player.MaxMagicFind)
                MagicFind = Player.MaxMagicFind;

            //this.Items[0] = GetJunkItem();

            int NextIndex = 0;
            for (int ecx = 0; ecx < this.Items.Length; ecx++)
            {
                if (ecx < 3)
                    IsGuarenteed = true;
                else
                    IsGuarenteed = false;

                this.Items[NextIndex] = GetRandomItem(EntityType, EnemyLevel, MagicFind, IsGuarenteed);
                if (this.Items[ecx] != Item.None)
                    NextIndex++;
            }
        }

        protected override Item GetRandomItem(EntityType EntityType, int EnemyLevel, float MagicFind, bool IsGuarenteed)
        {
            int Chance = Random.Next(0, 101); //0 to 100

            if (Chance > 80 || IsGuarenteed) //20% chance per item
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

    }
}
