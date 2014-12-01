using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River
{
    class StorageInventory : StandardInventory
    {
        public const int StorageStepSize = Item.IconSize;
        public const int StorageRowSize = 8;
        public const int StorageSlots = 64;

        public StorageInventory(Int32 InventoryID)
            : base(InventoryID)
        {

        }

        protected override void Initialize()
        {
            Items = new Item[StorageSlots];

            BasePosition.X = (Main.BackBufferWidth / 2 - StorageRowSize * Item.IconSize) / 2;
            BasePosition.Y = Item.IconSize;

            TextDrawPosition.X = BasePosition.X + Item.IconSize * 3;
            TextDrawPosition.Y = BasePosition.Y + (StorageSlots / StorageRowSize) * Item.IconSize + Item.IconSize;
        }

    }
}
