using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River
{
    class EnchantingInventory : StandardInventory
    {
        private int EnchantingSlots = 1;

        public EnchantingInventory() :
            base()
        {
            //Base will just call our initialize
        }

        protected override void Initialize()
        {
            this.Items = new Item[EnchantingSlots];

            BasePosition.X = (Main.BackBufferWidth / 2 - Item.IconSize) / 2;
            BasePosition.Y = Item.IconSize * 3;

            TextDrawPosition.X = BasePosition.X + Item.IconSize * 1;
            TextDrawPosition.Y = BasePosition.Y + (InventorySlots / InventoryRowSize) * Item.IconSize + Item.IconSize;
        }
    }
}
