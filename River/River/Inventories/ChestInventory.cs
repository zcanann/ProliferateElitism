using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River
{
    public enum ChestType
    {
        Chest,
        Lockbox,
    }

    class ChestInventory : LootInventory
    {
        public ChestInventory(ChestType ChestType, Int32 MyInventoryID)
            : base(MyInventoryID)
        {
            GenerateOnLoot = true;
        }

        private bool Generated = false;

        public bool IsGenerated()
        {
            return Generated;
        }

        public override void GenerateDrops(int BaseLevel, int AmountGuarenteed = 0)
        {
            base.GenerateDrops(BaseLevel, 2);
        }

    }
}
