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
        private int _Dexterity;
        private int _Strength;
        private int _Intellect;
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
        public int Dexterity
        {
            get { return _Dexterity; }
            set { if (value < 0) value = 0; _Dexterity = value; }
        }
        public int Strength
        {
            get { return _Strength; }
            set { if (value < 0) value = 0; _Strength = value; }
        }
        public int Intellect
        {
            get { return _Intellect; }
            set { if (value < 0) value = 0; _Intellect = value; }
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
            if (this.Strength != 0)
                ItemCount++;
            if (this.Dexterity != 0)
                ItemCount++;
            if (this.Intellect != 0)
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
            if (this.Strength != 0)
                ItemInfo[CurrentIndex++] = "Strength: " + Strength.ToString("N0");
            if (this.Dexterity != 0)
                ItemInfo[CurrentIndex++] = "Dexterity: " + Dexterity.ToString("N0");
            if (this.Intellect != 0)
                ItemInfo[CurrentIndex++] = "Intellect: " + Intellect.ToString("N0");

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
            BuyPrice = (long)(((int)Quality + 1) *
                (Vitality + Intellect + Dexterity + Strength + Armor + (int)Attack + ItemLevel));

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
            Strength = ((int)Quality) * (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2));
            Dexterity = ((int)Quality) * (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2));
            Intellect = ((int)Quality) * (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2));
            Vitality = ((int)Quality) * (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2));

            Armor = ((int)Quality + 2) * (WorkingItemLevel + Random.Next(WorkingItemLevel / 2));
            Attack = ((int)Quality + 2) * (WorkingItemLevel + Random.Next(WorkingItemLevel / 2));

            MagicFind = (float)Math.Round(((int)Quality) *
                (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2)) / 300f, 2);
            GoldFind = (float)Math.Round(((int)Quality) *
                (WorkingItemLevel + Random.Next(-WorkingItemLevel / 2, WorkingItemLevel / 2)) / 300f, 2);

            TryKillPrimaryStat();
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
            if (Random.Next(0, 100000) < 100000) //90% chance success wat
            {
                Enchantments++;

                PrimaryStatEnchant(ref _Strength);
                PrimaryStatEnchant(ref _Intellect);
                PrimaryStatEnchant(ref _Dexterity);
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

        private enum Weight { Strength, Dexterity, Intellect, Vitality };
        private string GenerateName()
        {
            string GeneratedName = "";

            Weight PrimaryWeight = Weight.Strength;
            Weight SecondaryWeight = Weight.Dexterity;

            //Find weighted stat
            if (Vitality >= Strength && Vitality >= Dexterity && Vitality >= Intellect)
                PrimaryWeight = Item.Weight.Vitality;
            else if (Strength >= Vitality && Strength >= Dexterity && Strength >= Intellect)
                PrimaryWeight = Item.Weight.Strength;
            else if (Dexterity >= Vitality && Dexterity >= Strength && Dexterity >= Intellect)
                PrimaryWeight = Item.Weight.Dexterity;
            else if (Intellect >= Vitality && Intellect >= Strength && Intellect >= Dexterity)
                PrimaryWeight = Item.Weight.Intellect;

            switch (PrimaryWeight)
            {
                case Weight.Vitality:
                    if (Strength >= Dexterity && Strength >= Intellect)
                        SecondaryWeight = Item.Weight.Strength;
                    else if (Dexterity >= Strength && Dexterity >= Intellect)
                        SecondaryWeight = Item.Weight.Dexterity;
                    else if (Intellect >= Strength && Intellect >= Dexterity)
                        SecondaryWeight = Item.Weight.Intellect;
                    break;
                case Weight.Strength:
                    if (Vitality >= Dexterity && Vitality >= Intellect)
                        SecondaryWeight = Item.Weight.Vitality;
                    else if (Dexterity >= Vitality && Dexterity >= Intellect)
                        SecondaryWeight = Item.Weight.Dexterity;
                    else if (Intellect >= Vitality && Intellect >= Dexterity)
                        SecondaryWeight = Item.Weight.Intellect;
                    break;
                case Weight.Dexterity:
                    if (Vitality >= Strength && Vitality >= Intellect)
                        SecondaryWeight = Item.Weight.Vitality;
                    else if (Strength >= Vitality && Strength >= Intellect)
                        SecondaryWeight = Item.Weight.Strength;
                    else if (Intellect >= Vitality && Intellect >= Strength)
                        SecondaryWeight = Item.Weight.Intellect;
                    break;
                case Weight.Intellect:
                    if (Vitality >= Strength && Vitality >= Dexterity)
                        SecondaryWeight = Item.Weight.Vitality;
                    else if (Strength >= Vitality && Strength >= Dexterity)
                        SecondaryWeight = Item.Weight.Strength;
                    else if (Dexterity >= Vitality && Dexterity >= Strength)
                        SecondaryWeight = Item.Weight.Intellect;
                    break;
            }

            //Check for a 0 of a primary stat
            switch (SecondaryWeight)
            {
                case Weight.Dexterity:
                    if (Dexterity == 0)
                        SecondaryWeight = PrimaryWeight;
                    break;
                case Weight.Strength:
                    if (Strength == 0)
                        SecondaryWeight = PrimaryWeight;
                    break;
                case Weight.Intellect:
                    if (Intellect == 0)
                        SecondaryWeight = PrimaryWeight;
                    break;
                case Weight.Vitality:
                    if (Vitality == 0)
                        SecondaryWeight = PrimaryWeight;
                    break;
            }

            switch (Quality)
            {
                case QualityType.White:
                    break;
                case QualityType.Yellow:
                case QualityType.Orange:
                case QualityType.Blue:
                    switch (PrimaryWeight)
                    {
                        case Weight.Vitality:
                            switch (SecondaryWeight)
                            {
                                case Weight.Vitality:
                                    GeneratedName = "Protector's ";
                                    break;
                                case Weight.Strength:
                                    GeneratedName = "Guardian's ";
                                    break;
                                case Weight.Dexterity:
                                    GeneratedName = "Defender's ";
                                    break;
                                case Weight.Intellect:
                                    GeneratedName = "Sentinel's ";
                                    break;
                            }
                            break;
                        case Weight.Strength:
                            switch (SecondaryWeight)
                            {
                                case Weight.Strength:
                                    GeneratedName = "Barbarian's ";
                                    break;
                                case Weight.Vitality:
                                    GeneratedName = "Hero's ";
                                    break;
                                case Weight.Dexterity:
                                    GeneratedName = "Champion's ";
                                    break;
                                case Weight.Intellect:
                                    GeneratedName = "Fighter's ";
                                    break;
                            }
                            break;
                        case Weight.Dexterity:
                            switch (SecondaryWeight)
                            {
                                case Weight.Dexterity:
                                    GeneratedName = "Thief's ";
                                    break;
                                case Weight.Vitality:
                                    GeneratedName = "Outlaw's ";
                                    break;
                                case Weight.Strength:
                                    GeneratedName = "Bandit's ";
                                    break;
                                case Weight.Intellect:
                                    GeneratedName = "Pilferer's ";
                                    break;
                            }
                            break;
                        case Weight.Intellect:
                            switch (SecondaryWeight)
                            {
                                case Weight.Intellect:
                                    GeneratedName = "Wizard's ";
                                    break;
                                case Weight.Vitality:
                                    GeneratedName = "Sorcerer's ";
                                    break;
                                case Weight.Strength:
                                    GeneratedName = "Seer's ";
                                    break;
                                case Weight.Dexterity:
                                    GeneratedName = "Scribe's ";
                                    break;
                            }
                            break;
                    }
                    break;
            }

            switch (Quality)
            {
                case QualityType.Yellow:
                    GeneratedName += "Legendary ";
                    break;
                case QualityType.Orange:
                    GeneratedName += "Godly ";
                    break;
            }

            switch (Slot)
            {
                case SlotType.Chest:
                    switch (Class)
                    {
                        case EntityType.Warrior:
                            GeneratedName += "Breastplate";
                            break;
                        case EntityType.Magician:
                            GeneratedName += "Robe";
                            break;
                        default:
                        case EntityType.Bandit:
                            GeneratedName += "Tunic";
                            break;
                    }
                    break;
                case SlotType.Feet:
                    switch (Class)
                    {
                        case EntityType.Warrior:
                            GeneratedName += "Greaves";
                            break;
                        case EntityType.Magician:
                            GeneratedName += "Boots";
                            break;
                        default:
                        case EntityType.Bandit:
                            GeneratedName += "Boots";
                            break;
                    }
                    break;
                case SlotType.Hands:
                    switch (Class)
                    {
                        case EntityType.Warrior:
                            GeneratedName += "Gauntlets";
                            break;
                        case EntityType.Magician:
                            GeneratedName += "Gloves";
                            break;
                        default:
                        case EntityType.Bandit:
                            GeneratedName += "Gloves";
                            break;
                    }
                    break;
                case SlotType.Head:
                    switch (Class)
                    {
                        case EntityType.Warrior:
                            GeneratedName += "Helm";
                            break;
                        case EntityType.Magician:
                            GeneratedName += "Hat";
                            break;
                        default:
                        case EntityType.Bandit:
                            GeneratedName += "Hood";
                            break;
                    }
                    break;
                case SlotType.Legs:
                    switch (Class)
                    {
                        case EntityType.Warrior:
                            GeneratedName += "Legplates";
                            break;
                        case EntityType.Magician:
                            GeneratedName += "Pants";
                            break;
                        default:
                        case EntityType.Bandit:
                            GeneratedName += "Leggings";
                            break;
                    }
                    break;
                case SlotType.Offhand:
                    switch (Class)
                    {
                        case EntityType.Warrior:
                            GeneratedName += "Shield";
                            break;
                        case EntityType.Magician:
                            GeneratedName += "Orb";
                            break;
                        default:
                        case EntityType.Bandit:
                            GeneratedName += "Quiver";
                            break;
                    }
                    break;
                case SlotType.Weapon:
                    switch (Class)
                    {
                        case EntityType.Warrior:
                            GeneratedName += "Sword";
                            break;
                        case EntityType.Magician:
                            GeneratedName += "Staff";
                            break;
                        default:
                        case EntityType.Bandit:
                            GeneratedName += "Dagger";
                            break;
                    }
                    break;
                default:
                    GeneratedName += Slot.ToString();
                    break;
            }

            return GeneratedName;
        }

        //Each item starts off being randomized with all 4 base stats -- here there is a chance to remove up to two
        private void TryKillPrimaryStat()
        {
            int KillCount = 0;
            int KillMax = 3;
            int KillMin = 0;

            if (Quality == QualityType.Orange)
            {
                KillMax = 1;
                KillMin = 0;
            }
            if (Quality == QualityType.Yellow)
            {
                KillMax = 2;
                KillMin = 1;
            }
            if (Quality == QualityType.Blue)
            {
                KillMax = 3;
                KillMin = 2;
            }
            //White items don't even have main stats
            if (Quality == QualityType.White)
                return;


            //Randomly generate order in which to try to kill off stats
            int[] TestOrder = new int[4];
            for (int ecx = 0; ecx < 4; ecx++)
                while (true)
                {
                    int RandomIndex = Random.Next(0, 4);
                    if (TestOrder[RandomIndex] == 0)
                    {
                        TestOrder[RandomIndex] = ecx + 1;
                        break;
                    }
                }

            int Index = 0;
        SelectNext:

            //Check if done testing or too many killed
            if (Index >= 4 || KillCount >= KillMax)
                return;

            switch (TestOrder[Index++])
            {
                case 1:
                    goto Str;
                case 2:
                    goto Int;
                case 3:
                    goto Dex;
                case 4:
                    goto Vit;
                default:
                    return;
            }

        Str:
            if (Random.Next(0, 100) < 60 || KillCount < KillMin)
            {
                Strength = 0;
                KillCount++;
            }
            goto SelectNext;
        Int:
            if (Random.Next(0, 100) < 60 || KillCount < KillMin)
            {
                Intellect = 0;
                KillCount++;
            }
            goto SelectNext;
        Dex:
            if (Random.Next(0, 100) < 60 || KillCount < KillMin)
            {
                Dexterity = 0;
                KillCount++;
            }
            goto SelectNext;
        Vit:
            if (Random.Next(0, 100) < 40 || KillCount < KillMin)
            {
                Vitality = 0;
                KillCount++;
            }
            goto SelectNext;

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
