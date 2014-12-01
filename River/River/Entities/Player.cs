using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace River
{
    public enum EntityType
    {
        Warrior,
        Bandit,
        Magician,

        Skeleton,
        Goblin,
        Slime,
        Zombie,
        Ogre,
        Elemental,
    }

    class Player : Entity
    {

        private Texture2D PlayerTexture;

        public int SecondarySelection = 1;
        public int DefensiveSelection = -1;
        public int SpecialSelection = -1;

        public StandardInventory Inventory;
        public Equipment Equipment;

        private const float PrimaryStatToAtt = 0.2f; //ex 5 strength = 1 att

        public bool NearLootable = false;
        public bool NearShop = false;
        public bool NearStorage = false;
        public bool NearEnchanter = false;

        public LootInventory LootingInventory;

        public List<FloatingNumber> FloatingGoldNumbers = new List<FloatingNumber>();
        public List<FloatingNumber> FloatingExpNumbers = new List<FloatingNumber>();

        public const int ExpPerLevel = 5000;

        //Player unique stats:
        protected int BasePrimary = 5;
        protected int BaseVitality = 5;

        public const float MaxMagicFind = 5f;
        public static float MagicFind = 5f; //TODO stop being lazy
        private float GoldFind = 0f;

        protected int BonusAttack;
        protected int Armor;
        protected int BonusPrimary;
        protected int BonusVitality;

        public string[] StatText = new string[7];

        public Player(Vector2 Position, Level LevelPTR, EntityType Class)
            : base(Position, LevelPTR)
        {
            this.Class = Class;
            Skill = new Skill(LevelPTR, Class, this);
            Inventory = new StandardInventory(GameDB.GetPlayerInventoryID(Class.ToString()));
            Equipment = new Equipment(LevelPTR, GameDB.GetEquipmentInventoryID());
            SetStatText();
        }

        //Loads texture based on class
        public void LoadConditionalContent(ContentManager Content, IGraphicsDeviceService Graphics)
        {
            switch (Class)
            {
                case EntityType.Warrior:
                    PlayerTexture = Content.Load<Texture2D>(@"Textures\Characters\Player_Warrior");
                    SpriteAnimation = new SpriteAnimation(PlayerTexture);
                    SpriteAnimation.DrawOffset = new Vector2(-56, -82);
                    break;
                case EntityType.Bandit:
                    PlayerTexture = Content.Load<Texture2D>(@"Textures\Characters\Player_Thief");
                    SpriteAnimation = new SpriteAnimation(PlayerTexture);
                    SpriteAnimation.DrawOffset = new Vector2(-56, -82);
                    break;
                case EntityType.Magician:
                    PlayerTexture = Content.Load<Texture2D>(@"Textures\Characters\Player_Magician");
                    SpriteAnimation = new SpriteAnimation(PlayerTexture);
                    SpriteAnimation.DrawOffset = new Vector2(-68, -88);
                    break;
            }

            int Size = 128;
            SpriteAnimation.AddAnimation("WalkWest", 0, Size * 0, Size, Size, 4, 0.1f);
            SpriteAnimation.AddAnimation("WalkNorthWest", 0, Size * 1, Size, Size, 4, 0.1f);
            SpriteAnimation.AddAnimation("WalkNorth", 0, Size * 2, Size, Size, 4, 0.1f);
            SpriteAnimation.AddAnimation("WalkNorthEast", 0, Size * 3, Size, Size, 4, 0.1f);
            SpriteAnimation.AddAnimation("WalkEast", 0, Size * 4, Size, Size, 4, 0.1f);
            SpriteAnimation.AddAnimation("WalkSouthEast", 0, Size * 5, Size, Size, 4, 0.1f);
            SpriteAnimation.AddAnimation("WalkSouth", 0, Size * 6, Size, Size, 4, 0.1f);
            SpriteAnimation.AddAnimation("WalkSouthWest", 0, Size * 7, Size, Size, 4, 0.1f);

            SpriteAnimation.AddAnimation("DeadWest", Size * 7, Size * 0, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("DeadNorthWest", Size * 7, Size * 1, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("DeadNorth", Size * 7, Size * 2, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("DeadNorthEast", Size * 7, Size * 3, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("DeadEast", Size * 7, Size * 4, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("DeadSouthEast", Size * 7, Size * 5, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("DeadSouth", Size * 7, Size * 6, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("DeadSouthWest", Size * 7, Size * 7, Size, Size, 1, 0.2f);

            SpriteAnimation.AddAnimation("IdleWest", 0, Size * 0, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("IdleNorthWest", 0, Size * 1, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("IdleNorth", 0, Size * 2, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("IdleNorthEast", 0, Size * 3, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("IdleEast", 0, Size * 4, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("IdleSouthEast", 0, Size * 5, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("IdleSouth", 0, Size * 6, Size, Size, 1, 0.2f);
            SpriteAnimation.AddAnimation("IdleSouthWest", 0, Size * 7, Size, Size, 1, 0.2f);

            SpriteAnimation.AddAnimation("AttkWest", Size * 4, Size * 0, Size, Size, 3, 0.15f, "IdleWest");
            SpriteAnimation.AddAnimation("AttkNorthWest", Size * 4, Size * 1, Size, Size, 3, 0.15f, "IdleNorthWest");
            SpriteAnimation.AddAnimation("AttkNorth", Size * 4, Size * 2, Size, Size, 3, 0.15f, "IdleNorth");
            SpriteAnimation.AddAnimation("AttkNorthEast", Size * 4, Size * 3, Size, Size, 3, 0.15f, "IdleNorthEast");
            SpriteAnimation.AddAnimation("AttkEast", Size * 4, Size * 4, Size, Size, 3, 0.15f, "IdleEast");
            SpriteAnimation.AddAnimation("AttkSouthEast", Size * 4, Size * 5, Size, Size, 3, 0.15f, "IdleSouthEast");
            SpriteAnimation.AddAnimation("AttkSouth", Size * 4, Size * 6, Size, Size, 3, 0.15f, "IdleSouth");
            SpriteAnimation.AddAnimation("AttkSouthWest", Size * 4, Size * 7, Size, Size, 3, 0.15f, "IdleSouthWest");

            SpriteAnimation.Position = Position;
            SpriteAnimation.CurrentAnimation = "WalkEast";
            SpriteAnimation.IsAnimating = true;

            LoadGeneralContent(Content, Graphics);
        }

        public void AddEquipmentStats(Item Item)
        {
            if (Item == null)
                return;

            //Grab stats from item
            BonusAttack += Item.Attack;
            Armor += Item.Armor;
            BonusPrimary += Item.Primary;
            BonusVitality += Item.Vitality;
            GoldFind += Item.GoldFind;
            MagicFind += Item.MagicFind;

            if (Item.Buff != null)
                ActiveBuffs.Add(Item.Buff);

            SetStatText();
        }

        public void RemoveEquipmentStats(Item Item)
        {
            if (Item == null)
                return;

            //Take away stats from buff
            BonusAttack -= Item.Attack;
            Armor -= Item.Armor;
            BonusPrimary -= Item.Primary;
            BonusVitality -= Item.Vitality;
            GoldFind -= Item.GoldFind;
            MagicFind -= Item.MagicFind;

            if (Item.Buff != null)
                ActiveBuffs.Remove(Item.Buff);

            SetStatText();
        }

        private void SetStatText()
        {
            StatText[0] = "Attack: " + (BaseAttack + BonusAttack).ToString();
            StatText[1] = "Attack Speed: " + (AttackSpeed).ToString() + "%";
            StatText[2] = "Armor: " + (Armor).ToString();
            StatText[3] = "Magic Find: +" + ((int)(100f * MagicFind)).ToString() + "%";
            StatText[4] = "Gold Find: +" + ((int)(100f * GoldFind)).ToString() + "%";
            StatText[5] = "Vitality: " + (BaseVitality + BonusVitality).ToString();
            switch (Class)
            {
                case EntityType.Bandit:
                    StatText[6] = "Dexterity: " + (BasePrimary + BonusPrimary).ToString();
                    break;
                case EntityType.Magician:
                    StatText[6] = "Intellect: " + (BasePrimary + BonusPrimary).ToString();
                    break;
                case EntityType.Warrior:
                    StatText[6] = "Strength: " + (BasePrimary + BonusPrimary).ToString();
                    break;
            }

        }

        public override float ComputeDamage(float PercentWeaponAttack)
        {
            float Damage = base.ComputeDamage(PercentWeaponAttack);

            Damage -= (BasePrimary + BonusPrimary) * PrimaryStatToAtt;

            return Damage;
        }

        private void FinishLooting()
        {
            if (LevelPTR.LastLootedEnemy != null)
                LevelPTR.LastLootedEnemy.LoadLootSparkle();

            LevelPTR.SendPrioritizedToBack(LootingInventory);
            LootingInventory = LevelPTR.FindNearbyLoot();
        }

        private void OpenNearbyInventory()
        {
            if (LootingInventory != null) //Shouldn't ever be null, depends if I fixed it. Just in case.
            {
                if (LootingInventory.GenerateOnLoot == true)
                {
                    LootingInventory.GenerateDrops(LevelValue);
                }

                //Open both inventory and other
                MenuManager.OpenLootMenu(true);
                MenuManager.OpenInventoryMenu(false);
                //Pass in the inventories to the swap helper class
                SwapHelper.Connect(LevelPTR.Player.Inventory);
                SwapHelper.Connect(LevelPTR.Player.LootingInventory);
                SwapHelper.SetDisconnectCallBack(new SwapHelper.SwapDisconnect(FinishLooting));

            }
        }

        private Vector2 MoveDir;
        public void Update(GameTime GameTime)
        {
            if (IsAlive)
            {
                MoveDir = Vector2.Zero;

                //MOVEMENT
                MoveDir.X = Main.GamePadState.ThumbSticks.Left.X;
                MoveDir.Y = -Main.GamePadState.ThumbSticks.Left.Y;

                //Overwrite with keyboard input
                if (Main.KeyboardState.IsKeyDown(Keys.Up))
                    MoveDir.Y = -1;
                else if (Main.KeyboardState.IsKeyDown(Keys.Down))
                    MoveDir.Y = 1;
                if (Main.KeyboardState.IsKeyDown(Keys.Left))
                    MoveDir.X = -1;
                else if (Main.KeyboardState.IsKeyDown(Keys.Right))
                    MoveDir.X = 1;


                if (MoveInDirection(GameTime, MoveDir)) //Move function returns true if position changes
                {
                    //Bind position to screen
                    Position.X = MathHelper.Clamp(Position.X, 0, Camera.WorldWidth);
                    Position.Y = MathHelper.Clamp(Position.Y, 0, Camera.WorldHeight);

                    if (LevelPTR.CheckForExitTile())
                        return;

                    LootingInventory = LevelPTR.FindNearbyLoot();
                }

            }

            //ATTACKING

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
            //!Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
            {
                if (NearLootable)
                    OpenNearbyInventory();
                else if (!IsAttacking)
                {
                    SkillType CastSkill = Skill.GetSkill(0, CastButton.A);

                    Attack();
                    Skill.PlayerCast(CastSkill, Position, GetDirectionVector());
                }
            }

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.X))
            //!Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.X))
            {
                if (!IsAttacking)
                {
                    SkillType CastSkill = Skill.GetSkill(SecondarySelection, CastButton.X);
                    if (CastSkill != SkillType.None)
                    {
                        Attack();
                        Skill.PlayerCast(CastSkill, Position, GetDirectionVector());
                    }
                }
            }

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.B))
            //!Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.B))
            {
                if (!IsAttacking)
                {
                    SkillType CastSkill = Skill.GetSkill(DefensiveSelection, CastButton.B);

                    if (CastSkill != SkillType.None)
                    {
                        Attack();
                        Skill.PlayerCast(CastSkill, Position, GetDirectionVector());
                    }
                }
            }

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Y))
            //!Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Y))
            {
                if (!IsAttacking)
                {
                    SkillType CastSkill = Skill.GetSkill(SpecialSelection, CastButton.Y);

                    if (CastSkill != SkillType.None)
                    {
                        Attack();
                        Skill.PlayerCast(CastSkill, Position, GetDirectionVector());
                    }
                }
            }


            UpdateFloatingGoldText(GameTime);
            UpdateFloatingExpText(GameTime);

            UpdateCamera();
            UpdateGeneral(GameTime);
        }

        private void UpdateFloatingGoldText(GameTime GameTime)
        {
            //Remove dead ones
            int eax = 0;
        Loop:
            for (; eax < FloatingGoldNumbers.Count; eax++)
            {
                FloatingGoldNumbers[eax].DecreaseDuration(GameTime.ElapsedGameTime.Milliseconds);

                if (FloatingGoldNumbers[eax].GetDuration() <= 0f)
                {
                    //Removing an index in the middle of a loop is problematic.
                    //Using goto to escape and cycle through the leftovers is a good way to avoid issues
                    FloatingGoldNumbers.RemoveAt(eax);
                    goto Loop; //Don't increment loop counter -- just exit the loop
                }
            }
        }

        private void UpdateFloatingExpText(GameTime GameTime)
        {
            //Remove dead ones
            int eax = 0;
        Loop:
            for (; eax < FloatingExpNumbers.Count; eax++)
            {
                FloatingExpNumbers[eax].DecreaseDuration(GameTime.ElapsedGameTime.Milliseconds);

                if (FloatingExpNumbers[eax].GetDuration() <= 0f)
                {
                    //Removing an index in the middle of a loop is problematic.
                    //Using goto to escape and cycle through the leftovers is a good way to avoid issues
                    FloatingExpNumbers.RemoveAt(eax);
                    goto Loop; //Don't increment loop counter -- just exit the loop
                }
            }
        }


        //Updates/scrolls camera based on position of player
        public void UpdateCamera()
        {
            Vector2 ScreenPosition = Camera.WorldToScreen(Position);

            //X Scrolling
            if (ScreenPosition.X < Camera.XScrollDistance)
                Camera.Move(new Vector2(ScreenPosition.X - Camera.XScrollDistance, 0));
            else if (ScreenPosition.X > (Main.BackBufferWidth - Camera.XScrollDistance))
                Camera.Move(new Vector2(ScreenPosition.X - (Main.BackBufferWidth - Camera.XScrollDistance), 0));

            //Y Scrolling
            if (ScreenPosition.Y < Camera.YScrollDistance)
                Camera.Move(new Vector2(0, ScreenPosition.Y - Camera.YScrollDistance));
            else if (ScreenPosition.Y > (Main.BackBufferHeight - Camera.YScrollDistance))
                Camera.Move(new Vector2(0, ScreenPosition.Y - (Main.BackBufferHeight - Camera.YScrollDistance)));
        }

        //private Vector2 GoldDrawPos = new Vector2(920, Main.BackBufferHeight - 124);
        public void DrawGoldFloatingText(SpriteBatch SpriteBatch)
        {
            float TimeFraction;

            int YOffset = -LevelPTR.LevelMap.GetOverallHeight(Position);
            int NewYOffset;

            //Draw any floating numbers caused by damage/healing
            for (int ecx = 0; ecx < FloatingGoldNumbers.Count; ecx++)
            {
                TimeFraction = FloatingGoldNumbers[ecx].GetDuration() / FloatingGoldNumbers[ecx].GetMaxDuration();

                NewYOffset = YOffset - 112 + (int)(128 * TimeFraction);

                SpriteBatch.DrawString(FloatingNumberFont,
                    FloatingGoldNumbers[ecx].GetValue(),
                    Camera.WorldToScreen(SpriteAnimation.Position) + SpriteAnimation.DrawOffset +
                     new Vector2(SpriteAnimation.Width / 2 - NumberFontWidth * FloatingGoldNumbers[ecx].GetValue().Length, NewYOffset),
                    // new Vector2(GoldDrawPos.X, GoldDrawPos.Y - 96 + (int)(128 * TimeFraction)),
                    //Color is multiplied by time division of durations (0f-1f) to create opacity
                    Color.Yellow * TimeFraction);
            }
        }

        public void AddGoldExperience(long Gold, int KillExp)
        {
            Gold = (long)(Gold * (1f + GoldFind));
            this.Gold += Gold;
            FloatingGoldNumbers.Add(new FloatingNumber("+" + Gold.ToString(), 1000f, false));

            if (LevelValue < LevelCap)
            {
                Experience += KillExp;
                FloatingExpNumbers.Add(new FloatingNumber("+EXP", 1000f, false));

                //Level up players
                while (Experience >= ExpPerLevel)
                {
                    //TODO: PARTICLE EFFECT FOR LEVELING
                    Experience -= ExpPerLevel;
                    if (++LevelValue == LevelCap)
                        Experience = ExpPerLevel;
                }
            }
        }

        //private Vector2 ExpDrawPos = new Vector2(340, Main.BackBufferHeight - 124);
        public void DrawExpFloatingText(SpriteBatch SpriteBatch)
        {
            float TimeFraction;

            int YOffset = -LevelPTR.LevelMap.GetOverallHeight(Position);
            int NewYOffset;

            //Draw any floating numbers caused by damage/healing
            for (int ecx = 0; ecx < FloatingExpNumbers.Count; ecx++)
            {
                TimeFraction = FloatingExpNumbers[ecx].GetDuration() / FloatingExpNumbers[ecx].GetMaxDuration();

                NewYOffset = YOffset - 128 + (int)(128 * TimeFraction);

                SpriteBatch.DrawString(FloatingNumberFont,
                    FloatingExpNumbers[ecx].GetValue(),
                     Camera.WorldToScreen(SpriteAnimation.Position) + SpriteAnimation.DrawOffset +
                     new Vector2(SpriteAnimation.Width / 2 - NumberFontWidth * FloatingExpNumbers[ecx].GetValue().Length, NewYOffset),
                    //new Vector2(ExpDrawPos.X, ExpDrawPos.Y - 96 + (int)(128 * TimeFraction)),
                    //Color is multiplied by time division of durations (0f-1f) to create opacity
                    Color.Cyan * TimeFraction);
            }
        }

    }
}
