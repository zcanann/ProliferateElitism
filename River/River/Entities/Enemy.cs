using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Emitters;
using ProjectMercury.Renderers;
using ProjectMercury;
using ProjectMercury.Modifiers;

namespace River
{
    class Enemy : Entity
    {
        private static Texture2D Skeleton;
        private static Texture2D Goblin;
        private static Texture2D Slime;
        private static Texture2D Zombie;
        private static Texture2D Ogre;
        private static Texture2D Elemental;

        private bool IsBoss = false;

        public LootInventory Inventory;

        public EntityType Type;
        public float AgroRadius = 480f;
        public float AttackRadius = 112f;//112f;
        private bool HasAgro = false;

        private CircleEmitter LootSparkle = null;

        private static SpriteBatchRenderer ParticleRenderer = null;
        //protected static Texture2D ParticleTexture;

        public Enemy(Vector2 Position, Level LevelPTR, EntityType Type, int ExpForKill)
            : base(Position, LevelPTR)
        {
            this.Type = Type;
            
            switch (Type)
            {
                case EntityType.Goblin:
                    SpriteAnimation = new SpriteAnimation(Goblin);
                    SpriteAnimation.DrawOffset = new Vector2(-56, -82);
                    break;
                case EntityType.Skeleton:
                    SpriteAnimation = new SpriteAnimation(Skeleton);
                    SpriteAnimation.DrawOffset = new Vector2(-56, -82);
                    Speed = 0.8f;
                    break;
                case EntityType.Slime:
                    SpriteAnimation = new SpriteAnimation(Slime);
                    SpriteAnimation.DrawOffset = new Vector2(-56, -82);
                    break;
                case EntityType.Zombie:
                    SpriteAnimation = new SpriteAnimation(Zombie);
                    SpriteAnimation.DrawOffset = new Vector2(-56, -82);
                    break;
                case EntityType.Ogre:
                    SpriteAnimation = new SpriteAnimation(Ogre);
                    SpriteAnimation.DrawOffset = new Vector2(-56, -82);
                    break;
                case EntityType.Elemental:
                    SpriteAnimation = new SpriteAnimation(Elemental);
                    SpriteAnimation.DrawOffset = new Vector2(-56, -82);
                    break;
            }


            Inventory = new LootInventory();
            Gold = (long)Random.Next(LevelID, LevelID * 15);
            Experience = ExpForKill;

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
        }

        public static void LoadContent(ContentManager Content, IGraphicsDeviceService Graphics)
        {
            Skeleton = Content.Load<Texture2D>(@"Textures\Enemies\Skeleton");
            Goblin = Content.Load<Texture2D>(@"Textures\Enemies\Goblin");
            Slime = Content.Load<Texture2D>(@"Textures\Enemies\Slime");
            Zombie = Content.Load<Texture2D>(@"Textures\Enemies\Zombie");
            Ogre = Content.Load<Texture2D>(@"Textures\Enemies\Ogre");
            Elemental = Content.Load<Texture2D>(@"Textures\Enemies\Elemental");

            LoadGeneralContent(Content, Graphics);

            ParticleTexture = Content.Load<Texture2D>(@"Textures\UI\AttributeParticle");

            ParticleRenderer = new SpriteBatchRenderer();
            ParticleRenderer.GraphicsDeviceService = Graphics;
            ParticleRenderer.LoadContent(Content);
        }

        public void LoadLootSparkle()
        {
            Item.QualityType BestQualityItem = Item.QualityType.White;

            int NullCount = 0;
            for (int ecx = 0; ecx < Inventory.Items.Length; ecx++)
                if (Inventory.Items[ecx] != null)
                {
                    if (Inventory.Items[ecx].Quality > BestQualityItem)
                        BestQualityItem = Inventory.Items[ecx].Quality;
                }
                else
                    NullCount++;

            if (NullCount == Inventory.Items.Length)
            {
                LootSparkle = null;
                return;
            }

            LootSparkle = new CircleEmitter();
            LootSparkle.Radius = 24;
            LootSparkle.ReleaseQuantity = 2;
            LootSparkle.ReleaseOpacity = new VariableFloat { Value = 0.2f, Variation = 0f };

            switch (BestQualityItem)
            {
                case Item.QualityType.White:
                    LootSparkle.ReleaseQuantity = 1;
                    LootSparkle.ReleaseScale = new VariableFloat { Value = 3f, Variation = 0f };
                    LootSparkle.ReleaseColour = Color.White.ToVector3();
                    break;
                case Item.QualityType.Blue:
                    LootSparkle.ReleaseQuantity = 2;
                    LootSparkle.ReleaseScale = new VariableFloat { Value = 5f, Variation = 0f };
                    LootSparkle.ReleaseColour = Color.Blue.ToVector3();
                    break;
                case Item.QualityType.Yellow:
                    LootSparkle.ReleaseQuantity = 2;
                    LootSparkle.ReleaseScale = new VariableFloat { Value = 5f, Variation = 0f };
                    LootSparkle.ReleaseColour = Color.Yellow.ToVector3();
                    break;
                case Item.QualityType.Orange:
                    LootSparkle.ReleaseQuantity = 3;
                    LootSparkle.ReleaseScale = new VariableFloat { Value = 5f, Variation = 0f };
                    LootSparkle.ReleaseColour = Color.Orange.ToVector3();
                    break;
            }

            LinearGravityModifier LGM = new LinearGravityModifier();
            LGM.Gravity = new Vector2(0, -256f);
            LootSparkle.Modifiers.Add(LGM);

            //Initialize
            LootSparkle.ParticleTexture = ParticleTexture;
            LootSparkle.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            LootSparkle.Initialise(200, 0.4f);
        }

        private float Distance;
        private Vector2 MoveDir;
        public void Update(GameTime GameTime, Player Player)
        {
            if (IsAlive)
            {

                //Check if blinded or player shrouded
                if (!HasAgro)
                {
                    if (!Player.Cloaked &&
                        !Blinded &&
                        Tile.SquareTest(Player.Position, Position, AgroRadius) &&
                        (Tile.CircleTest(Player.Position, Position, AgroRadius, out Distance)))
                    {
                        MoveDir = UnitCircle.ComputeAngle(Position, Player.Position);
                        if (HasLoS(MoveDir, Distance))
                            HasAgro = true;
                    }
                }

                if (HasAgro)
                {
                    Tile.CircleTest(Player.Position, Position, AgroRadius, out Distance);
                    Agro(GameTime, Distance, Player);
                }
                else
                    Wander(GameTime);
            }
            else if (LootSparkle != null)
            {
                LootSparkle.Trigger(Camera.WorldToScreen(Position));
                LootSparkle.Update((float)GameTime.ElapsedGameTime.TotalSeconds);
            }

            UpdateGeneral(GameTime);
        }

        public override void ChangeHealth(int Offset)
        {
            //Only add floating numbers to a living target
            if (Offset != 0)
            {
                if (Health > 0)
                {
                    bool IsNegative = false;
                    if (Offset < 0)
                        IsNegative = true;

                    FloatingNumbers.Add(new FloatingNumber(Offset.ToString(), Random.Next(1000, 1500), IsNegative));
                }

                Health += Offset;

                //Death check
                if (Health <= 0 && IsAlive)
                {
                    IsAlive = false;
                    Health = 0;
                    SpriteAnimation.CurrentAnimation = GetDeathAnimation();
                    Inventory.GenerateDrops(Type, LevelID, Player.MagicFind, IsBoss);
                    LoadLootSparkle();
                    LevelPTR.GivePlayerGoldExp(Gold, Experience);

                    //Reorganize loot priority on corpses if this creature contributed any drops
                    if (Inventory.Items != null)
                        LevelPTR.OrganizeLootPriority(Inventory);
                }
            }
        }

        private void Agro(GameTime GameTime, float Distance, Player Player)
        {
            if (Distance > AttackRadius)
            {
                MoveDir = UnitCircle.ComputeAngle(Position, Player.Position);
                //Move using the new vector
                this.MoveInDirection(GameTime, MoveDir);
            }
            else
            {
                //Enemy Attack

                if (!IsAttacking)
                {
                    Attack();

                    //Generic swing
                    /*
                    LevelPTR.DamageEmitters.Add(
                        new DamageEmitter(Position,
                        GetDirectionVector(),
                        LevelPTR,
                        250f,
                        24f,
                        1.25f,
                        1,
                        true,
                        false,
                        SpriteAnimation.CurrentAnimation.Substring(4));
                     */
                }
            }
        }

        private float WanderReset = 0f;      //in ms
        private float WanderDuration = 0f;   //in ms
        private void Wander(GameTime GameTime)
        {
            //Update reset timer if it is active
            if (WanderReset > 0f)
                WanderReset -= GameTime.ElapsedGameTime.Milliseconds;

            if (WanderReset <= 0f && WanderDuration > 0f)
            {
                WanderDuration -= GameTime.ElapsedGameTime.Milliseconds;
                if (!this.MoveInDirection(GameTime, MoveDir))
                    ResetWander(true);
            }
            else
                this.MoveInDirection(GameTime, Vector2.Zero);

            if (WanderDuration <= 0f)
                ResetWander(false);

        }

        private void ResetWander(bool Immediate)
        {

            if (Immediate)
                WanderReset = 0f;
            else
                WanderReset = Random.Next(1000, 3000);
            WanderDuration = Random.Next(400, 1300);


            //Assign random move vector -- random between 0-1 in x/y
            MoveDir = new Vector2((float)Random.NextDouble(),
                (float)Random.NextDouble());

            //50% chance to reverse signs
            if (Random.Next(0, 2) == 0)
                MoveDir.X *= -1;
            if (Random.Next(0, 2) == 0)
                MoveDir.Y *= -1;
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            base.Draw(SpriteBatch);

            if (!IsAlive && LootSparkle != null)
                ParticleRenderer.RenderEmitter(LootSparkle, SpriteBatch);
        }

    }
}
