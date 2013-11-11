using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River
{
    class WeaponRackInventory : LootInventory
    {
        public WeaponRackInventory()
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

            if (MagicFind > Player.MaxMagicFind)
                MagicFind = Player.MaxMagicFind;

            //this.Items[0] = GetJunkItem();

            int NextIndex = 0;
            for (int ecx = 0; ecx < 3; ecx++)
            {
                if (ecx == 0)
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
                return new Items.Weapon(EnemyLevel, MagicFind);

            return null;
        }

    }
}
