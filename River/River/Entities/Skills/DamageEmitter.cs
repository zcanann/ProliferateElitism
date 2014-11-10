using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Emitters;
using ProjectMercury.Renderers;
using ProjectMercury.Modifiers;

using Mercury.ParticleEngine.Modifiers;
using ProjectMercury;

namespace River
{

    /// <summary>
    /// All things that cause damage: projectiles, swings, traps, etc
    /// </summary>
    class DamageEmitter
    {
        public List<int> HitTargets = new List<int>(); //Holds indexes of hit enemies (-1 for player) -- won't hit them again.
        protected bool HasHitEntity = false;

        public Point MapPoint;
        public SpriteAnimation SpriteAnimation;
        public Vector2 SpawnPosition;
        public Vector2 Position;
        public Buff Debuff;

        private SkillType SkillType;

        public Entity ParentEntity;
        protected Level LevelPTR;
        protected Vector2 Direction;
        protected bool IsAlive = true;
        protected bool PlayerOwned;
        protected bool MultiTarget;
        protected float MaxDuration;
        protected float Duration;
        protected float Radius;
        protected float Speed;
        protected float Damage;

        private float DeathDelay = 0f;  // Keep effect going while the emitter exists

        public EllipseEmitter MainEffect = null;
        public EllipseEmitter SecondaryEffect = null;
        
        private static SpriteBatchRenderer ParticleRenderer = null;
        protected static Texture2D ParticleTexture;

        public DamageEmitter(Entity ParentEntity, Level LevelPTR, Vector2 Position, Vector2 Direction,
            float Duration, float Radius, float Speed, float Damage,
            bool MultiTarget, bool PlayerOwned,

            Texture2D Texture, SkillType SkillType,

            Buff Debuff = null)
        {
            this.ParentEntity = ParentEntity;
            this.LevelPTR = LevelPTR;
            this.Position = Position;
            this.SpawnPosition = Position;
            this.Direction = Direction;
            this.Duration = Duration;
            this.MaxDuration = Duration;
            this.Radius = Radius;
            this.Speed = Speed;
            this.Damage = Damage;
            this.MultiTarget = MultiTarget;
            this.PlayerOwned = PlayerOwned;
            this.Debuff = Debuff;

            this.SkillType = SkillType;

            SpriteAnimation = new SpriteAnimation(Texture);

            int Size;

            #region Set up animations manually based on how sprite sheet looks

            switch (SkillType)
            {
                case River.SkillType.Arrow:
                case River.SkillType.AoeShot:
                case River.SkillType.FlamingArrow:
                case River.SkillType.PlagueArrow:
                case River.SkillType.PiercingShot:
                case River.SkillType.Multishot:
                    Size = 64;
                    SpriteAnimation.AddAnimation("AttkWest", Size * 0, 0, Size, Size, 1, 0.1f);
                    SpriteAnimation.AddAnimation("AttkNorthWest", Size * 1, 0, Size, Size, 1, 0.1f);
                    SpriteAnimation.AddAnimation("AttkNorth", Size * 2, 0, Size, Size, 1, 0.1f);
                    SpriteAnimation.AddAnimation("AttkNorthEast", Size * 3, 0, Size, Size, 1, 0.1f);
                    SpriteAnimation.AddAnimation("AttkEast", Size * 4, 0, Size, Size, 1, 0.1f);
                    SpriteAnimation.AddAnimation("AttkSouthEast", Size * 5, 0, Size, Size, 1, 0.1f);
                    SpriteAnimation.AddAnimation("AttkSouth", Size * 6, 0, Size, Size, 1, 0.1f);
                    SpriteAnimation.AddAnimation("AttkSouthWest", Size * 7, 0, Size, Size, 1, 0.1f);
                    SpriteAnimation.DrawOffset = new Vector2(-28, -54);
                    break;
                case SkillType.LightningShield:
                case SkillType.IceShield:
                    Size = 128;
                    SpriteAnimation.AddAnimation("AttkWest", 0, 0, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkNorthWest", 0, 0, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkNorth", 0, 0, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkNorthEast", 0, 0, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkEast", 0, 0, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkSouthEast", 0, 0, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkSouth", 0, 0, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkSouthWest", 0, 0, Size, Size, 4, 0.1f);
                    SpriteAnimation.DrawOffset = new Vector2(-56, -88);
                    break;
                //Standard attack format
                default:
                    Size = 64;
                    SpriteAnimation.AddAnimation("AttkWest", 0, Size * 0, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkNorthWest", 0, Size * 1, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkNorth", 0, Size * 2, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkNorthEast", 0, Size * 3, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkEast", 0, Size * 4, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkSouthEast", 0, Size * 5, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkSouth", 0, Size * 6, Size, Size, 4, 0.1f);
                    SpriteAnimation.AddAnimation("AttkSouthWest", 0, Size * 7, Size, Size, 4, 0.1f);
                    SpriteAnimation.DrawOffset = new Vector2(-32, -54);
                    break;
            }

            #endregion

            SpriteAnimation.IsAnimating = true;
            SpriteAnimation.Position = Position;
            SpriteAnimation.CurrentAnimation = "Attk" + Entity.GetDirectionAnimation(Direction);
        }

        protected void SetEffect(ref EllipseEmitter Effect, float Radius, Color ReleaseColor, int ReleaseQuantity,
            VariableFloat ReleaseScale, float FadeInitial, float FadeSpeed, bool Ring = false)
        {
            ControlledFade ControlledFade = new ControlledFade();
            ControlledFade.SetInitial(FadeInitial);
            ControlledFade.SetSpeed(FadeSpeed);

            Effect = new EllipseEmitter(Radius, Ring);
            Effect.ReleaseColour = ReleaseColor.ToVector3();
            Effect.ReleaseQuantity = ReleaseQuantity;
            Effect.ReleaseScale = ReleaseScale;

            if (ControlledFade != null)
                Effect.Modifiers.Add(ControlledFade);
        }


        public static void LoadContent(ContentManager Content, IGraphicsDeviceService Graphics)
        {
            ParticleTexture = Content.Load<Texture2D>(@"Textures\UI\AttributeParticle");

            ParticleRenderer = new SpriteBatchRenderer();
            ParticleRenderer.GraphicsDeviceService = Graphics;
            ParticleRenderer.LoadContent(Content);
        }

        public virtual void Update(GameTime GameTime)
        {
            // Check if not in the process of being terminated
            if (DeathDelay <= 0f)
            {
                if (SecondaryEffect != null)
                    SecondaryEffect.Trigger(Camera.WorldToScreen(Position));
                if (MainEffect != null)
                    MainEffect.Trigger(Camera.WorldToScreen(Position));
            }

            // Always update emitters that exist
            if (SecondaryEffect != null)
                SecondaryEffect.Update((float)GameTime.ElapsedGameTime.TotalSeconds);
            if (MainEffect != null)
                MainEffect.Update((float)GameTime.ElapsedGameTime.TotalSeconds);

            // UPDATE DEATH TIMER (if marked as such)
            if (DeathDelay > 0f)
            {
                DeathDelay -= GameTime.ElapsedGameTime.Milliseconds;
                if (DeathDelay <= 0f)
                    IsAlive = false;

                return;
            }

            if (!IsAlive)
                return;

            Duration -= GameTime.ElapsedGameTime.Milliseconds;

            if (SpriteAnimation != null)
                SpriteAnimation.Update(GameTime);

            if (Duration > 0)
                //Still active, update if it has a direction vector
                if (Direction != Vector2.Zero)
                {
                    Vector2 MoveDir = Vector2.Multiply(Direction, Speed * GameTime.ElapsedGameTime.Milliseconds / Main.SpeedConst);

                    //Apply
                    Position.X += MoveDir.X * 2;
                    Position.Y += MoveDir.Y;
                }

            if (Duration <= 0)
            {
                //Emitter timed out; mark this as dead
                Kill();
                Duration = 0f;
            }

            SpriteAnimation.SetPosition(Position);
        }

        #region public methods

        public virtual bool Intersects(Vector2 ComparePosition, int Index)
        {
            //Don't bother if its dead
            if (!IsAlive || DeathDelay > 0)
                return false;

            //Check if already hit
            for (int ecx = 0; ecx < HitTargets.Count; ecx++)
                if (Index == HitTargets[ecx])
                    return false;

            if ((!HasHitEntity || MultiTarget) && Tile.IntersectionTest(ComparePosition, Position, Radius))
            {
                //Kill emitter if it isn't multitarget
                if (!MultiTarget)
                    Kill();

                HasHitEntity = true;

                //Add hit target to the list
                HitTargets.Add(Index);

                return true;
            }

            //No collision
            return false;
        }

        public float GetRadius()
        {
            return Radius;
        }

        public bool IsMultiTarget()
        {
            return MultiTarget;
        }

        public float GetDuration()
        {
            return Duration;
        }

        public float GetMaxDuration()
        {
            return MaxDuration;
        }

        public float GetSpeed()
        {
            return Speed;
        }

        public bool IsPlayerOwned()
        {
            return PlayerOwned;
        }

        public void Kill()
        {
            if (DeathDelay <= 0f)
                DeathDelay = 4000f;
            Speed = 0f;
        }

        public bool GetIsAlive()
        {
            return IsAlive;
        }

        public bool IsMarkedDead()
        {
            //Return false if marked for death
            if (DeathDelay > 0f)
                return true;

            return false;
        }

        public float GetDamage()
        {
            return Damage;
        }

        public Texture2D GetTexture()
        {
            return SpriteAnimation.Texture;
        }

        public Vector2 GetDirection()
        {
            return Direction;
        }

        public SkillType GetSkillType()
        {
            return SkillType;
        }

        public Buff GetDebuff()
        {
            if (Debuff == null)
                return null;

            //Make a copy so that yolo
            Buff BuffCopy = new Buff(Debuff.GetName(), Debuff.GetSpeed(), Debuff.GetState(), Debuff.GetDurationMax(),
                Debuff.GetTickDurationMax(), Debuff.GetTickHealthOffset(), Debuff.GetSpeedMultiplier());

            return BuffCopy;
        }

        #endregion


        #region Protected common functions

        //Adjust position to front of player
        protected static void AdjustPositionToFront(ref Vector2 Position, Vector2 Direction)
        {
            Position.X += Direction.X * Tile.TileStepX / 2;
            Position.Y += Direction.Y * Tile.TileStepY;
        }

        //Return a new position in front of player
        protected static Vector2 AdjustPositionToFront(Vector2 Position, Vector2 Direction)
        {
            Position.X += Direction.X * Tile.TileStepX / 2;
            Position.Y += Direction.Y * Tile.TileStepY;
            return Position;
        }

        //Returns a position on the side of a standard projectile
        protected static Vector2 GetSidePosition(bool First, Vector2 Position,
            Vector2 DirectionVector, float AngleOffset,
            out Vector2 NewDirection)
        {
            //Calc angle
            NewDirection = new Vector2();

            float Angle = UnitCircle.GetCircleAngle(DirectionVector);

            if (First)
            {
                //Get new coords
                NewDirection.X = (float)Math.Cos(Angle - AngleOffset);
                NewDirection.Y = (float)Math.Sin(Angle - AngleOffset);

                Position.X += NewDirection.X * Tile.TileStepX / 2;
                Position.Y += NewDirection.Y * Tile.TileStepY;
                return Position;
            }
            else
            {
                NewDirection.X = (float)Math.Cos(Angle + AngleOffset);
                NewDirection.Y = (float)Math.Sin(Angle + AngleOffset);

                Position.X += NewDirection.X * Tile.TileStepX / 2;
                Position.Y += NewDirection.Y * Tile.TileStepY;
                return Position;
            }
        }

        protected static void LatchToParent(ref Vector2 EmitterPos, Vector2 ParentPosition)
        {
            EmitterPos = ParentPosition;
        }

        protected static int GetLatchTarget(ref List<int> HitTargets)
        {
            //Grab ID of hit target
            return HitTargets[HitTargets.Count - 1];
        }
        protected static void LatchToTarget(Level LevelPTR, ref Vector2 EmitterPos, int LatchEnemyIndex)
        {
            //Latch to whatever was hit
            if (LatchEnemyIndex != -1)
                EmitterPos = LevelPTR.Enemies[LatchEnemyIndex].Position;
        }

        protected static void CreateSideShots(Level LevelPTR, Entity ParentEntity, DamageEmitter DamageEmitter, float AngleOffset)
        {
            Vector2 SideBlastVector;

            bool Left = false;
            for (int Side = 1; Side <= 2; Side++)
            {
                LevelPTR.DamageEmitters.Add(
                    new DamageEmitter(
                        ParentEntity,
                        LevelPTR,
                    GetSidePosition(Left, DamageEmitter.Position, DamageEmitter.GetDirection(),
                            AngleOffset, out SideBlastVector),
                    SideBlastVector,
                    DamageEmitter.GetMaxDuration(),
                    DamageEmitter.GetRadius(),
                    DamageEmitter.GetSpeed(),
                    DamageEmitter.GetDamage(),
                    DamageEmitter.IsMultiTarget(),
                    DamageEmitter.IsPlayerOwned(),
                    DamageEmitter.GetTexture(),
                    DamageEmitter.GetSkillType(),
                    DamageEmitter.GetDebuff()));

                //Copy hit targets for side shots
                LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].HitTargets = DamageEmitter.HitTargets;

                //Copy effects
                LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].MainEffect = (EllipseEmitter)DamageEmitter.MainEffect.DeepCopy();
                LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].SecondaryEffect = (EllipseEmitter)DamageEmitter.SecondaryEffect.DeepCopy();

                LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].MainEffect.Initialise(750, 20);
                LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].SecondaryEffect.Initialise(750, 20);

                Left = !Left;
            }

            //Put leading skill in front (have to do this after since we use its position to calculate
            //the position of the two side attacks)
            AdjustPositionToFront(ref DamageEmitter.Position, DamageEmitter.GetDirection());
        }
        #endregion

        Matrix TransFormSwag = new Matrix(1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
        public virtual void Draw(SpriteBatch SpriteBatch, Level LevelPTR)
        {
            //SpriteAnimation.Draw(SpriteBatch, 0, -LevelPTR.LevelMap.GetOverallHeight(Position));
            if (MainEffect != null)
                ParticleRenderer.RenderEmitter(MainEffect, SpriteBatch);
            if (SecondaryEffect != null)
                ParticleRenderer.RenderEmitter(SecondaryEffect, SpriteBatch);
        }

    }
}
