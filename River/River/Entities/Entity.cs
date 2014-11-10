using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Emitters;
using ProjectMercury.Renderers;
using ProjectMercury;

namespace River
{

    class Entity
    {
        public EntityType Class;
        protected List<FloatingNumber> FloatingNumbers = new List<FloatingNumber>();
        protected static SpriteFont FloatingNumberFont;
        public SpriteAnimation SpriteAnimation;
        protected static Random Random = new Random();

        protected Level LevelPTR;
        protected Skill Skill;

        //STATE
        public List<Buff> ActiveBuffs = new List<Buff>();
        public Vector2 Position;
        public Point MapPoint;      //Used to assist in drawing (class Level)
        public bool IsAttacking = false;
        public bool IsAlive = true;
        protected bool Blinded = false;
        private bool Poisoned = false;
        private bool Burning = false;
        private bool Chilled = false;
        private bool Frozen = false;
        public bool Cloaked = false;

        private static CircleEmitter BlindEffect = null;
        private static CircleEmitter PoisonEffect = null;
        private static CircleEmitter BurnEffect = null;
        private static CircleEmitter ChillEffect = null;
        private static CircleEmitter FrozenEffect = null;
        private static CircleEmitter CloakEffect = null;

        private static SpriteBatchRenderer ParticleRenderer = null;
        protected static Texture2D ParticleTexture;

        //Constant stat ratios
        private const int VitalityToHP = 10;
        private const int IntellectToMP = 3; //Or not

        //ATTRIBUTES
        public const int LevelCap = 20;
        public int LevelID = 1;

        //For enemies these will serve as given/dropped

        public int Experience;
        public long Gold = 75;

        protected int MaxHealth = 100;
        protected int Health = 100;
        protected int MaxMana = 100;
        protected int Mana = 100;

        protected float BaseAttack = 10f; // Damage done multiplier
        protected float Speed = 1f; //1f = 100% -- all speed changes aside from enemy presets should go through buffs
        protected float AttackSpeed = 1f;
        protected float BuffSpeedMultiplier = 1f; // 0 = all movement nullified (done via buffs)

        //private float BonusAttackMultiplier;

        public Entity(Vector2 Position, Level LevelPTR)
        {
            this.Position = Position;
            this.LevelPTR = LevelPTR;
        }

        protected static void LoadGeneralContent(ContentManager Content, IGraphicsDeviceService Graphics)
        {
            FloatingNumberFont = Content.Load<SpriteFont>(@"Fonts\FloatingNumbers");

            ParticleTexture = Content.Load<Texture2D>(@"Textures\UI\AttributeParticle");

            ParticleRenderer = new SpriteBatchRenderer();
            ParticleRenderer.GraphicsDeviceService = Graphics;
            ParticleRenderer.LoadContent(Content);

            ////////////
            //MAIN:
            ////////////
            BurnEffect = new CircleEmitter();
            BurnEffect.Radius = 32;
            BurnEffect.ReleaseColour = Color.Orange.ToVector3();
            BurnEffect.ReleaseQuantity = 12;
            BurnEffect.ReleaseRotation = new VariableFloat { Value = 0f, Variation = 0.0f };
            BurnEffect.ReleaseScale = new VariableFloat { Value = 12f, Variation = 4f };
            BurnEffect.ReleaseOpacity = new VariableFloat { Value = 1f, Variation = 0f };

            ControlledFade MainOM = new ControlledFade();
            MainOM.SetInitial(0.05f);
            MainOM.SetSpeed(100f);
            BurnEffect.Modifiers.Add(MainOM);

            //Initialize
            BurnEffect.ParticleTexture = ParticleTexture;
            BurnEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            BurnEffect.Initialise(1000, 2f);
        }

        #region Update

        public static void UpdateStaticEmitters(GameTime GameTime)
        {
            BurnEffect.Update((float)GameTime.ElapsedGameTime.TotalSeconds);
        }

        //Update function shared by all entities
        protected void UpdateGeneral(GameTime GameTime)
        {
            //Update and apply all buffs
            UpdateBuffs(GameTime);

            //Update animations
            SpriteAnimation.Update(GameTime);

            //Update state
            if (FinishedAttacking())
                IsAttacking = false;

            //Update floating text
            UpdateFloatingText(GameTime);
        }

        private void UpdateFloatingText(GameTime GameTime)
        {
            //Remove dead ones
            int eax = 0;
        Loop:
            for (; eax < FloatingNumbers.Count; eax++)
            {
                FloatingNumbers[eax].DecreaseDuration(GameTime.ElapsedGameTime.Milliseconds);

                if (FloatingNumbers[eax].GetDuration() <= 0f)
                {
                    //Removing an index in the middle of a loop is problematic.
                    //Using goto to escape and cycle through the leftovers is a good way to avoid issues
                    FloatingNumbers.RemoveAt(eax);
                    goto Loop; //Don't increment loop counter -- just exit the loop
                }
            }
        }

        private void UpdateBuffs(GameTime GameTime)
        {
            //Update buff timers & remove expired
            int BuffIndex = 0;
        Loop:
            for (; BuffIndex < ActiveBuffs.Count; BuffIndex++)
                if (ActiveBuffs[BuffIndex].GetDurationMax() != Buff.Indefinite)
                {
                    ActiveBuffs[BuffIndex].ReduceDuration(GameTime.ElapsedGameTime.Milliseconds);
                    if (ActiveBuffs[BuffIndex].GetDurationCurrent() <= 0f)
                    {
                        ActiveBuffs.RemoveAt(BuffIndex);
                        goto Loop;
                    }
                }

            //Update tick timers
            for (int ecx = 0; ecx < ActiveBuffs.Count; ecx++)
            {
                if (ActiveBuffs[ecx].GetTickDurationMax() != Buff.Indefinite)
                {
                    ActiveBuffs[ecx].ReduceTickDurationCurrent(GameTime.ElapsedGameTime.Milliseconds);

                    if (ActiveBuffs[ecx].GetTickDurationCurrent() <= 0f)
                    {
                        //Apply damage/healing for the tick
                        ChangeHealth((int)ActiveBuffs[ecx].GetTickHealthOffset());
                        ActiveBuffs[ecx].ResetTickTimer();
                    }
                }
            }

            //Reset and check if still true later
            Cloaked = false;
            Frozen = false;
            Chilled = false;
            Burning = false;
            Poisoned = false;
            BuffSpeedMultiplier = 1;

            //Set up player state based on buffs
            for (int ecx = 0; ecx < ActiveBuffs.Count; ecx++)
            {
                BuffSpeedMultiplier *= ActiveBuffs[ecx].GetSpeedMultiplier();
                switch (ActiveBuffs[ecx].GetState())
                {
                    case Buff.StateType.None:
                        break;
                    case Buff.StateType.Cloak:
                        Cloaked = true;
                        break;
                    case Buff.StateType.Freeze:
                        Frozen = true;
                        break;
                    case Buff.StateType.Chill:
                        Chilled = true;
                        break;
                    case Buff.StateType.Blind:
                        Blinded = true;
                        break;
                    case Buff.StateType.Burn:
                        Burning = true;
                        break;
                    case Buff.StateType.Poison:
                        Poisoned = true;
                        break;
                }
            }


            if (Cloaked)
                SpriteAnimation.Tint = Color.Black;
            else if (Frozen)
                SpriteAnimation.Tint = Color.Blue;
            else if (Blinded)
                SpriteAnimation.Tint = Color.Yellow;
            else if (Burning)
                SpriteAnimation.Tint = Color.Red;
                //BurnEffect.Trigger(Camera.WorldToScreen(Position));
            else if (Poisoned)
                SpriteAnimation.Tint = Color.Green;
            else if (Chilled)
                SpriteAnimation.Tint = Color.Blue;
            else
                SpriteAnimation.Tint = Color.White;

        }

        #endregion

        #region Private/Protected Animations, attacking, movement functions

        protected void Attack()
        {
            if (GetAttackAnimation() != SpriteAnimation.CurrentAnimation)
                SpriteAnimation.CurrentAnimation = GetAttackAnimation();

            IsAttacking = true;
        }

        protected string GetDeathAnimation()
        {
            {
                switch (SpriteAnimation.CurrentAnimation.Substring(4))
                {
                    case "North":
                        return "DeadNorth";
                    case "NorthEast":
                        return "DeadNorthEast";
                    case "NorthWest":
                        return "DeadNorthWest";
                    case "South":
                        return "DeadSouth";
                    case "SouthWest":
                        return "DeadSouthWest";
                    case "SouthEast":
                        return "DeadSouthEast";
                    case "West":
                        return "DeadWest";
                    case "East":
                        return "DeadEast";
                    default:
                        throw new Exception("Error in Entity.PlayDeathAnimation() -- unrecognized current animation.");
                }
            }

        }

        protected string GetAttackAnimation()
        {
            return "Attk" + SpriteAnimation.CurrentAnimation.Substring(4);
        }

        protected bool FinishedAttacking()
        {
            //Check if animation is an attacking one
            switch (SpriteAnimation.CurrentAnimation)
            {
                case "AttkNorth":
                case "AttkNorthEast":
                case "AttkNorthWest":
                case "AttkSouth":
                case "AttkSouthWest":
                case "AttkSouthEast":
                case "AttkWest":
                case "AttkEast":
                    return false;
            }

            //Must be done attacking
            return true;
        }

        private const float WeightSensitivity = 0.35f; //Determines how far is considered diagonal
        public static string GetDirectionAnimation(Vector2 MoveDir)
        {
            string Animation;

            /* DETERMINE PROPER ANIMATION:
             * Step 1: Determine if there is more weight in the x or y direction.
             * Step 2: Determine actual direction in primary direction
             * Step 3: Determine if there is significant weight to justify diagonal animation */

            if (Math.Abs(MoveDir.X) >= Math.Abs(MoveDir.Y)) //X
            {
                if (MoveDir.X > 0) //RIGHT
                {
                    //Check for significant diagonal weights
                    if (MoveDir.Y > WeightSensitivity)
                        Animation = "SouthEast";
                    else if (MoveDir.Y < -WeightSensitivity)
                        Animation = "NorthEast";
                    else
                        Animation = "East"; //No significant diagonal weight
                }
                else  //LEFT
                {
                    if (MoveDir.Y > WeightSensitivity)
                        Animation = "SouthWest";
                    else if (MoveDir.Y < -WeightSensitivity)
                        Animation = "NorthWest";
                    else
                        Animation = "West";
                }
            }
            else  //Y
            {
                if (MoveDir.Y > 0) //DOWN
                {
                    if (MoveDir.X > WeightSensitivity)
                        Animation = "SouthEast";
                    else if (MoveDir.X < -WeightSensitivity)
                        Animation = "SouthWest";
                    else
                        Animation = "South";
                }
                else    //UP
                {
                    if (MoveDir.X > WeightSensitivity)
                        Animation = "NorthEast";
                    else if (MoveDir.X < -WeightSensitivity)
                        Animation = "NorthWest";
                    else
                        Animation = "North";
                }
            }

            return Animation;
        } //End GetDirectionAnimation

        protected Vector2 GetDirectionVector()
        {
            switch (SpriteAnimation.CurrentAnimation.Substring(4))
            {
                case "North":
                    return new Vector2(0, -1);
                case "NorthEast":
                    return new Vector2(1, -1);
                case "NorthWest":
                    return new Vector2(-1, -1);
                case "South":
                    return new Vector2(0, 1);
                case "SouthWest":
                    return new Vector2(-1, 1);
                case "SouthEast":
                    return new Vector2(1, 1);
                case "West":
                    return new Vector2(-1, 0);
                case "East":
                    return new Vector2(1, 0);
                default:
                    throw new Exception("Error in Entity.GetAttackVector() -- unrecognized current animation.");
            }
        }

        #endregion

        #region Public Methods

        public virtual float ComputeDamage(float PercentWeaponAttack)
        {
            float Damage = PercentWeaponAttack * BaseAttack;

            Damage -= (float)Random.Next(-(int)(Damage / 10), (int)(Damage / 10) + 1);

            return -Damage;
        }

        public int GetMaxHealth()
        {
            return MaxHealth;
        }
        public int GetHealth()
        {
            return Health;
        }

        public virtual void ChangeHealth(int Offset)
        {
            //Only add floating numbers to a living target
            if (Offset != 0)
            {
                if (IsAlive && Health < MaxHealth)
                {
                    bool IsNegative = false;
                    if (Offset < 0)
                        IsNegative = true;

                    FloatingNumbers.Add(new FloatingNumber(Offset.ToString(), Random.Next(1000, 1500), IsNegative));
                }

                Health += Offset;

                if (Health > MaxHealth)
                    Health = MaxHealth;

                //Death check
                if (Health <= 0 && IsAlive)
                {
                    IsAlive = false;
                    Health = 0;
                    SpriteAnimation.CurrentAnimation = GetDeathAnimation();
                }
            }
        }

        private float DeltaH = 0f;
        private float LastHeight = 0f;
        private float DecrementVal = 0.1f;
        private const bool SlopeSpeedEnabled = false;
        //Returns true if moved
        public bool MoveInDirection(GameTime GameTime, Vector2 MoveDir)
        {

            if (IsAttacking)
                return false;

            //Check if their is insignificant weight in the X/Y direction
            if (Math.Abs(MoveDir.X) < WeightSensitivity && Math.Abs(MoveDir.Y) < WeightSensitivity)
            {
                SpriteAnimation.CurrentAnimation = "Idle" + SpriteAnimation.CurrentAnimation.Substring(4);
                return false;
            }

            MoveDir.X *= 2;

            //1 - Apply speed
            Vector2.Multiply(ref MoveDir, BuffSpeedMultiplier * Speed * GameTime.ElapsedGameTime.Milliseconds / Main.SpeedConst, out MoveDir);

            //2 - Apply speed adjustments based on slope of ground
            if (SlopeSpeedEnabled)
            {
                float GroundSlope = LevelPTR.LevelMap.GetSlopeHeightAtWorldPoint(Position + MoveDir);
                DeltaH = GroundSlope - LastHeight;
                LastHeight = GroundSlope;
                if (DeltaH > 0f)
                {
                    //Keep bound to a reasonable range
                    DeltaH = MathHelper.Clamp(DeltaH / 2, 0f, 1f);
                    //Apply the slow
                    MoveDir = Vector2.Multiply(MoveDir, DeltaH);
                }
            }

            //Prevent game-breaking speeds
            MoveDir.X = MathHelper.Clamp(MoveDir.X, -8f, 8f);
            MoveDir.Y = MathHelper.Clamp(MoveDir.Y, -4f, 4f);

            //Update animation
            if (MoveDir.Length() != 0)
            {
                string Animation = "Walk" + GetDirectionAnimation(MoveDir);
                if (SpriteAnimation.CurrentAnimation != Animation)
                    SpriteAnimation.CurrentAnimation = Animation;
            }
            else
                SpriteAnimation.CurrentAnimation = "Idle" + SpriteAnimation.CurrentAnimation.Substring(4);

            if (Main.CollisionDetection)
            {
                //WALKABLE TILE CHECK
                int Direction = 1; //Positive
                if (MoveDir.X < 0)
                    Direction = -1; //Negative

                //X
                //Try reducing how far moving before we declare we can't move that way
                while (!LevelPTR.LevelMap.IsWalkableCell(Position + new Vector2(MoveDir.X, 0)))
                {
                    MoveDir.X -= DecrementVal * Direction;

                    if ((MoveDir.X < 0 && Direction == 1) ||
                        (MoveDir.X > 0 && Direction == -1))
                    {
                        MoveDir.X = 0;
                        break;
                    }

                }

                //Y
                if (MoveDir.Y < 0)
                    Direction = -1; //Negative
                else
                    Direction = 1; //Positive
                while (!LevelPTR.LevelMap.IsWalkableCell(Position + new Vector2(0, MoveDir.Y)))
                {
                    MoveDir.Y -= DecrementVal * Direction;

                    if ((MoveDir.Y < 0 && Direction == 1) ||
                        (MoveDir.Y > 0 && Direction == -1))
                    {
                        MoveDir.Y = 0;
                        break;
                    }

                }

                //Height difference check
                //if (Math.Abs(LevelPTR.LevelMap.GetOverallHeight(Position) -
                //    LevelPTR.LevelMap.GetOverallHeight(Position + MoveDir)) > 10)
                //    MoveDir = Vector2.Zero;
            }

            if (MoveDir == Vector2.Zero)
                return false;
            else
            {
                //Apply
                Position.X += MoveDir.X;
                Position.Y += MoveDir.Y;
                SpriteAnimation.SetPosition(Position);
                return true;
            }

        } //End MoveInDirections

        private Vector2 StepDir;
        protected bool HasLoS(Vector2 MoveDir, float Distance)
        {

            StepDir.X = MoveDir.X;
            StepDir.Y = MoveDir.Y;

            float TraveledDist = 0f;

            float DistStep = (float)Math.Sqrt(MoveDir.X * MoveDir.X + MoveDir.Y * MoveDir.Y);

            //X
            //Try reducing how far moving before we declare we can't move that way
            while (TraveledDist < Distance)
            {
                if (!LevelPTR.LevelMap.IsWalkableCell(Position + StepDir))
                    return false;

                TraveledDist += DistStep;
                StepDir.X += MoveDir.X;
                StepDir.Y += MoveDir.Y;
            }

            return true;
        }

        #endregion

        protected const int NumberFontWidth = 4;
        public virtual void Draw(SpriteBatch SpriteBatch)
        {
            int YOffset = -LevelPTR.LevelMap.GetOverallHeight(Position);

            //Draw animation
            SpriteAnimation.Draw(SpriteBatch, 0, YOffset);

            Color DrawColor;
            int NewYOffset;
            float TimeFraction;

            ParticleRenderer.RenderEmitter(BurnEffect, SpriteBatch);

            //Draw any floating numbers caused by damage/healing
            for (int ecx = 0; ecx < FloatingNumbers.Count; ecx++)
            {
                if (FloatingNumbers[ecx].IsNegative())
                    DrawColor = Color.Red;
                else
                    DrawColor = Color.Green;

                TimeFraction = FloatingNumbers[ecx].GetDuration() / FloatingNumbers[ecx].GetMaxDuration();

                NewYOffset = YOffset - 96 + (int)(128 * TimeFraction);

                SpriteBatch.DrawString(FloatingNumberFont,
                    FloatingNumbers[ecx].GetValue(),
                     Camera.WorldToScreen(SpriteAnimation.Position) + SpriteAnimation.DrawOffset +
                     new Vector2(SpriteAnimation.Width / 2 - NumberFontWidth * FloatingNumbers[ecx].GetValue().Length, NewYOffset),
                    //Color is multiplied by time division of durations (0f-1f) to create opacity
                    DrawColor * TimeFraction);
            }

        }

    }

    /// <summary>
    /// Numbers that float as a result of damage/healing
    /// </summary>
    class FloatingNumber
    {
        private float MaxDuration;
        private float Duration;
        private string Value;
        private bool Negative;

        public FloatingNumber(string Value, float Duration, bool Negative)
        {
            this.Value = Value;
            this.MaxDuration = Duration;
            this.Duration = Duration;
            this.Negative = Negative;
        }

        public void DecreaseDuration(int Amount)
        {
            this.Duration -= Amount;
        }
        public string GetValue()
        {
            return Value;
        }

        public float GetDuration()
        {
            return Duration;
        }

        public float GetMaxDuration()
        {
            return MaxDuration;
        }

        public bool IsNegative()
        {
            return Negative;
        }

    }
}
