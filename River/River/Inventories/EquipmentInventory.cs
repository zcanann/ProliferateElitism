using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace River
{
    class Equipment : StandardInventory
    {
        public const int EquipmentRowSize = 3;
        public const int EquipmentSlots = 9;
        public const int EquipmentStepSize = 3 * Item.IconSize / 2;

        public static Texture2D[] GrayedItems = new Texture2D[9];

        private static Level LevelPTR;

        public Equipment(Level _LevelPTR)
            : base()
        {
            //Base will just call initialize

            LevelPTR = _LevelPTR;
        }

        protected override void Initialize()
        {
            this.Items = new Item[EquipmentSlots];

            //Set up base position
            this.BasePosition = new Vector2((Main.BackBufferWidth / 2 - Item.IconSize) / 2 - Item.IconSize , Item.IconSize);
            this.BasePosition.X -= EquipmentStepSize * (EquipmentRowSize / 2);

            TextDrawPosition.X = BasePosition.X + Item.IconSize * 1;
            TextDrawPosition.Y = BasePosition.Y + (InventorySlots / InventoryRowSize) * Item.IconSize + Item.IconSize;
        }

        public static void LoadContent(ContentManager Content)
        {
            GrayedItems[(int)Item.SlotType.Ring] = Content.Load<Texture2D>(@"Textures\Inventory\Grayed\RingGrayed");
            GrayedItems[(int)Item.SlotType.Head] = Content.Load<Texture2D>(@"Textures\Inventory\Grayed\HeadGrayed");
            GrayedItems[(int)Item.SlotType.Amulet] = Content.Load<Texture2D>(@"Textures\Inventory\Grayed\AmuletGrayed");
            GrayedItems[(int)Item.SlotType.Weapon] = Content.Load<Texture2D>(@"Textures\Inventory\Grayed\WeaponGrayed");
            GrayedItems[(int)Item.SlotType.Chest] = Content.Load<Texture2D>(@"Textures\Inventory\Grayed\ChestGrayed");
            GrayedItems[(int)Item.SlotType.Offhand] = Content.Load<Texture2D>(@"Textures\Inventory\Grayed\OffhandGrayed");
            GrayedItems[(int)Item.SlotType.Hands] = Content.Load<Texture2D>(@"Textures\Inventory\Grayed\HandsGrayed");
            GrayedItems[(int)Item.SlotType.Legs] = Content.Load<Texture2D>(@"Textures\Inventory\Grayed\LegsGrayed");
            GrayedItems[(int)Item.SlotType.Feet] = Content.Load<Texture2D>(@"Textures\Inventory\Grayed\FeetGrayed");
        }


        //Swap function where one inventory is always
        new public static void SwapItems(StandardInventory Inventory, int InventoryIndex, StandardInventory Equipment, int EquipmentIndex)
        {
            //Grab first item and store in Temp
            Item Temp = Inventory.Items[InventoryIndex];

            //Remove old item stats
            LevelPTR.Player.AddEquipmentStats(Equipment.Items[EquipmentIndex]);

            //Swap
            Inventory.Items[InventoryIndex] = Equipment.Items[EquipmentIndex];
            Equipment.Items[EquipmentIndex] = Temp;

            //Add new items stats 
            LevelPTR.Player.RemoveEquipmentStats(Equipment.Items[EquipmentIndex]);
        }


        public override void Draw(SpriteBatch SpriteBatch)
        {
            Vector2 DrawPosition = new Vector2(BasePosition.X, BasePosition.Y);

            for (int ecx = 0; ecx < Items.Length; ecx++)
            {
                if (Items[ecx] != Item.None)
                    Items[ecx].Draw(SpriteBatch, DrawPosition);
                else
                    SpriteBatch.Draw(GrayedItems[ecx], DrawPosition, Color.White);

                DrawPosition.X += EquipmentStepSize;

                if ((ecx + 1) % EquipmentRowSize == 0)
                {
                    DrawPosition.Y += EquipmentStepSize;
                    DrawPosition.X = BasePosition.X;
                }
            }
        } //End draw

    }
}
