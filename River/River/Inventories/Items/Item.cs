using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace River
{
    class Item
    {
        public enum SlotType
        {
            Ring,
            Head,
            Amulet,
            Weapon,
            Chest,
            Offhand,
            Hands,
            Legs,
            Feet,
            None,
        }

        protected enum SocketStateType
        {
            None,
            Empty,
            Filled
        }

        public enum QualityType
        {
            White = 0,
            Blue = 1,
            Yellow = 2,
            Orange = 3
        }

        public const Item None = null;
        private const int MaxEnchantments = 10;

        public SlotType Slot;
        public QualityType Quality;
        protected static Random Random = new Random();

        public const int IconSize = 64;
        public static SpriteFont ItemTextFont;

        public static Texture2D[] IconFrame = new Texture2D[4];
        public static Texture2D StatPanel;
        public static Texture2D ItemStatPanel;
        public static Texture2D GoldPanel;
        protected static Texture2D[] RingIcon = new Texture2D[1];
        protected static Texture2D[] HeadIcon = new Texture2D[1];
        protected static Texture2D[] AmuletIcon = new Texture2D[1];
        protected static Texture2D[] WeaponIcon = new Texture2D[1];
        protected static Texture2D[] ChestIcon = new Texture2D[1];
        protected static Texture2D[] OffhandIcon = new Texture2D[1];
        protected static Texture2D[] HandsIcon = new Texture2D[1];
        protected static Texture2D[] LegsIcon = new Texture2D[1];
        protected static Texture2D[] FeetIcon = new Texture2D[1];

        protected static Texture2D[] GemIcon = new Texture2D[16];

        protected Texture2D IconTexture;

        public Buff Buff;
        public Buff DebuffOnAttack;
        public Buff DebuffOnDefend;

        private static EntityType Class;

        private int Enchantments = 0;

        private int _Armor;
        private int _Primary;
        private int _Vitality;

        private string ItemName;
        public int ItemLevel;
        public int Attack;
        public float AttackSpeedBonus;

        public string[] ItemInfo;
        public long SellPrice;
        public long BuyPrice;
        public long EnchantPrice;

        public int Armor
        {
            get { return _Armor; }
            set { if (value < 0) value = 0; _Armor = value; }
        }
        public int Primary
        {
            get { return _Primary; }
            set { if (value < 0) value = 0; _Primary = value; }
        }
        public int Vitality
        {
            get { return _Vitality; }
            set { if (value < 0) value = 0; _Vitality = value; }
        }
        public float MagicFind;
        public float GoldFind;

        public Item(int EnemyLevel, float MagicFind, SlotType Slot)
        {
            this.Slot = Slot;
            this.RandomizeQuality(MagicFind);
            this.RandomizeStats(EnemyLevel);
            this.TryGiveBuff();
            this.SetIcon();
            this.SetPrice();
            this.SetText();
        }

        public static void LoadContent(ContentManager Content)
        {
            GoldPanel = Content.Load<Texture2D>(@"Textures\Inventory\GoldPanel");
            StatPanel = Content.Load<Texture2D>(@"Textures\Inventory\StatPanel");
            ItemStatPanel = Content.Load<Texture2D>(@"Textures\Inventory\ItemStatPanel");
            ItemTextFont = Content.Load<SpriteFont>(@"Fonts\FloatingNumbers");
            for (int ecx = 0; ecx < GemIcon.Length; ecx++)
                GemIcon[ecx] = Content.Load<Texture2D>(@"Textures\Inventory\Default");

            IconFrame[0] = Content.Load<Texture2D>(@"Textures\Inventory\FrameEmpty");
            IconFrame[1] = Content.Load<Texture2D>(@"Textures\Inventory\FrameBlue");
            IconFrame[2] = Content.Load<Texture2D>(@"Textures\Inventory\FrameYellow");
            IconFrame[3] = Content.Load<Texture2D>(@"Textures\Inventory\FrameOrange");
        }

        public static void LoadConditionalContent(ContentManager Content, EntityType PlayerClass)
        {
            Class = PlayerClass;
            switch (Class)
            {
                case EntityType.Bandit:
                    RingIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Ring0");
                    HeadIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Head0");
                    AmuletIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Amulet0");
                    WeaponIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Weapon0");
                    ChestIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Chest0");
                    OffhandIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Offhand0");
                    HandsIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Hands0");
                    LegsIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Legs0");
                    FeetIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Feet0");
                    break;

                case EntityType.Magician:
                    RingIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Ring0");
                    HeadIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Head0");
                    AmuletIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Amulet0");
                    WeaponIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Weapon0");
                    ChestIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Chest0");
                    OffhandIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Offhand0");
                    HandsIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Hands0");
                    LegsIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Legs0");
                    FeetIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Feet0");
                    break;

                default:
                case EntityType.Warrior:
                    RingIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Ring0");
                    HeadIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Head0");
                    AmuletIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Amulet0");
                    WeaponIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Weapon0");
                    ChestIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Chest0");
                    OffhandIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Offhand0");
                    HandsIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Hands0");
                    LegsIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Legs0");
                    FeetIcon[0] = Content.Load<Texture2D>(@"Textures\Inventory\Warrior\Feet0");
                    break;
            }
        }

        protected virtual void SetText()
        {
            int ItemCount = 2; //Name & price are guarenteed
            int CurrentIndex = 0;

            if (this.ItemLevel != 0)
                ItemCount++;
            if (this.Attack != 0)
                ItemCount++;
            if (this.AttackSpeedBonus != 0)
                ItemCount++;
            if (this.Armor != 0)
                ItemCount++;
            if (this.MagicFind != 0)
                ItemCount++;
            if (this.GoldFind != 0)
                ItemCount++;
            if (this.Primary != 0)
                ItemCount++;
            if (this.Vitality != 0)
                ItemCount++;
            if (this.Buff != null)
                ItemCount++;
            if (this.DebuffOnAttack != null)
                ItemCount++;
            if (this.DebuffOnDefend != null)
                ItemCount++;

            ItemInfo = new string[ItemCount];

            if (Enchantments > 0)
                ItemInfo[CurrentIndex++] = ItemName + " (" + Enchantments.ToString() + ")";
            else
                ItemInfo[CurrentIndex++] = ItemName;

            if (this.ItemLevel != 0)
                ItemInfo[CurrentIndex++] = "Level: " + ItemLevel.ToString();


            if (this.Attack != 0)
                ItemInfo[CurrentIndex++] = "Attack: " + Attack.ToString("N0");
            if (this.AttackSpeedBonus != 0)
                ItemInfo[CurrentIndex++] = "Attack Speed: " + AttackSpeedBonus.ToString();
            if (this.Armor != 0)
                ItemInfo[CurrentIndex++] = "Armor: " + Armor.ToString("N0");
            if (this.Vitality != 0)
                ItemInfo[CurrentIndex++] = "Vitality: " + Vitality.ToString("N0");

            switch (Class)
            {
                case EntityType.Bandit:
                    if (this.Primary != 0)
                        ItemInfo[CurrentIndex++] = "Dexterity: " + Primary.ToString("N0");
                    break;
                case EntityType.Warrior:
                    if (this.Primary != 0)
                        ItemInfo[CurrentIndex++] = "Strength: " + Primary.ToString("N0");
                    break;
                case EntityType.Magician:
                    if (this.Primary != 0)
                        ItemInfo[CurrentIndex++] = "Intellect: " + Primary.ToString("N0");
                    break;
            }



            if (this.MagicFind != 0)
                ItemInfo[CurrentIndex++] = "Magic Find: " + Math.Round((100f * MagicFind)).ToString() + "%";
            if (this.GoldFind != 0)
                ItemInfo[CurrentIndex++] = "Gold Find: " + Math.Round((100f * GoldFind)).ToString() + "%";

            if (this.Buff != null)
                ItemInfo[CurrentIndex++] = Buff.GetName();
            if (this.DebuffOnAttack != null)
                ItemInfo[CurrentIndex++] = DebuffOnAttack.GetName();
            if (this.DebuffOnDefend != null)
                ItemInfo[CurrentIndex++] = DebuffOnDefend.GetName();

            ItemInfo[CurrentIndex] = ""; //Reserved for price (which varies based on buying/selling/etc)

        }

        public void SetPrice()
        {
            BuyPrice = (long)(((int)Quality + 1) * (Vitality + Primary + Armor + (int)Attack + ItemLevel));

            SellPrice = BuyPrice / 4;

            EnchantPrice = BuyPrice * 8;
        }

        protected virtual void SetIcon() { } //Overloads take care of this


        public virtual void RandomizeStats(int EnemyLevel)
        {
            //Randomize item level
            ItemLevel = Random.Next(EnemyLevel - 5, EnemyLevel);

            if (ItemLevel < 1)
                ItemLevel = 1;
            if (ItemLevel > Entity.LevelCap)
                ItemLevel = Entity.LevelCap;

            //Low item levels produce awful randomization -- rather than messing with formulas treat them as a slightly
            //Higher leveled item (min of 6)
            int WorkingItemLevel = ItemLevel;
            if (WorkingItemLevel < 6)
                WorkingItemLevel = 6;

            //Randomize stats
            Primary = ((int)Quality) * (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2));
            Vitality = ((int)Quality) * (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2));

            Armor = ((int)Quality + 2) * (WorkingItemLevel + Random.Next(WorkingItemLevel / 2));
            Attack = ((int)Quality + 2) * (WorkingItemLevel + Random.Next(WorkingItemLevel / 2));

            MagicFind = (float)Math.Round(((int)Quality) *
                (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2)) / 300f, 2);
            GoldFind = (float)Math.Round(((int)Quality) *
                (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2)) / 300f, 2);

            ItemName = GenerateName();
        }

        private const int MinValForEnchant = 5; //Numbers < this and > 0 have issues with enchanting formula

        public bool CanEnchant()
        {
            if (Enchantments < MaxEnchantments)
                return true;

            return false;
        }
        public bool DoEnchant()
        {
            if (Random.Next() < 0.90) //90% chance success wat
            {
                Enchantments++;

                PrimaryStatEnchant(ref _Primary);
                PrimaryStatEnchant(ref _Vitality);
                PrimaryStatEnchant(ref _Armor);
                PrimaryStatEnchant(ref Attack);

                MagicFind += .01f;
                GoldFind += .01f;

                SetPrice();
                SetText();
                return true;
            }
            return false;

        }

        private void PrimaryStatEnchant(ref int Stat)
        {
            if (Stat > 0)
            {
                if (Stat < MinValForEnchant)
                    Stat = MinValForEnchant;

                Stat = Stat * 9 / 7;
            }
        }

        protected virtual void TryGiveBuff() { } //Overrides take care of this

        private string GenerateName()
        {
            string GeneratedName = Slot.ToString();

            return GeneratedName;
        }


        private const float OrangeChance = 0.25f / 100f; //.25
        private const float YellowChance = 4f / 100f;
        private const float BlueChance = 12f / 100f; //12
        //======= Remaining chance is white chance =======\\
        protected void RandomizeQuality(float MagicFind)
        {
            //Random between 1 and 100
            float QualityNum = (float)(Random.Next(0, 101)) / 100f;

            //Multiply by chance of finding a magic item
            float MagicMultiplier = 1f + MagicFind;

            if (QualityNum <= OrangeChance * MagicMultiplier)
                Quality = QualityType.Orange;
            else if (QualityNum <= (YellowChance + OrangeChance) * MagicMultiplier)
                Quality = QualityType.Yellow;
            else if (QualityNum <= (BlueChance + YellowChance + OrangeChance) * MagicMultiplier)
                Quality = QualityType.Blue;

            //======= Remaining chance is white chance =======\\
            else
                Quality = QualityType.White;

        }

        public void Draw(SpriteBatch SpriteBatch, Vector2 Position)
        {
            SpriteBatch.Draw(IconFrame[(int)Quality], Position, Color.White);
            SpriteBatch.Draw(IconTexture, Position, Color.White);
        }

        private static Color DrawColor;
        private const int TextStepSize = 24;
        public enum PriceDisplayType { Buy, Sell, Enchant };
        public void DrawText(SpriteBatch SpriteBatch, Vector2 Position, PriceDisplayType PriceDisplay)
        {
            if (Position.Y < Main.BackBufferHeight / 2)
                Position.Y += Item.IconSize / 2;
            else
                Position.Y -= (ItemInfo.Length + 1) * TextStepSize - Item.IconSize / 2;


            Position.X += Item.IconSize / 2;
            if (Position.X > Main.BackBufferWidth - Main.BackBufferWidth / 4)
                Position.X -= StatPanel.Width - Item.IconSize;

            SpriteBatch.Draw(Item.ItemStatPanel, new Rectangle((int)Position.X, (int)Position.Y,
                ItemStatPanel.Width, (ItemInfo.Length + 1) * TextStepSize), Color.White);

            Position.X += 12;
            Position.Y += 8;

            switch (Quality)
            {
                case QualityType.White:
                    DrawColor = Color.White;
                    break;
                case QualityType.Blue:
                    DrawColor = Color.Teal;
                    break;
                case QualityType.Yellow:
                    DrawColor = Color.Yellow;
                    break;
                case QualityType.Orange:
                    DrawColor = Color.DarkOrange;
                    break;
            }

            for (int ecx = 0; ecx < ItemInfo.Length - 1; ecx++)
            {
                SpriteBatch.DrawString(ItemTextFont, ItemInfo[ecx], Position, DrawColor);
                Position.Y += TextStepSize;
            }

            switch (PriceDisplay)
            {
                case PriceDisplayType.Buy:
                    SpriteBatch.DrawString(ItemTextFont, "Price: " + BuyPrice.ToString("N0"), Position, Color.Green);
                    break;
                case PriceDisplayType.Enchant:
                    if (Enchantments < MaxEnchantments)
                        SpriteBatch.DrawString(ItemTextFont, "Enchant Price: " + EnchantPrice.ToString("N0"), Position, Color.Purple);
                    else
                        SpriteBatch.DrawString(ItemTextFont, "Max Enchantments", Position, Color.Purple);
                    break;
                case PriceDisplayType.Sell:
                    SpriteBatch.DrawString(ItemTextFont, "Price: " + SellPrice.ToString("N0"), Position, Color.Green);
                    break;
            }

        }

    }
}
