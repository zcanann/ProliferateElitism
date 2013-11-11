﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Emitters;
using ProjectMercury;
using ProjectMercury.Modifiers;

namespace River.Skills
{
    class Fireball : DamageEmitter
    {

        private const int FollowShotCount = 2;
        private bool[] Created = new bool[FollowShotCount];
        private int CurrentIndex = 0;
        private float CreateDelayMax = 100f;
        private float CreateDelay = 0f;

        private bool Invisible = false;

        private const float SpawnSpeed = 2.5f;

        public Fireball(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 1000f,
            float Radius = 96f,
            float Speed = SpawnSpeed,
            float Damage = 0.4f,
            bool MultiTarget = false,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.FireBall)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType,

            new Buff("Fireball Burn", Buff.StateType.Burn, 4000f, 500f, -1f, 0.5f))
        {
            AdjustPositionToFront(ref this.Position, Direction);

            ////////////
            //MAIN:
            ////////////
            MainEffect = new CircleEmitter();
            MainEffect.Radius = 16;
            MainEffect.ReleaseColour = Color.Black.ToVector3();
            MainEffect.ReleaseQuantity = 12;
            MainEffect.ReleaseScale = new VariableFloat { Value = 16f, Variation = 4f };

            ControlledFade MainOM = new ControlledFade();
            MainOM.SetInitial(0.1f);
            MainOM.SetSpeed(100f);
            MainEffect.Modifiers.Add(MainOM);

            ////////////
            //SECONDARY:
            ////////////
            SecondaryEffect = new CircleEmitter();
            SecondaryEffect.Radius = 16;
            SecondaryEffect.ReleaseColour = Color.Orange.ToVector3();
            SecondaryEffect.ReleaseQuantity = 12;
            SecondaryEffect.ReleaseScale = new VariableFloat { Value = 32f, Variation = 4f };

            ControlledFade SecondaryOM = new ControlledFade();
            SecondaryOM.SetInitial(0.2f);
            SecondaryOM.SetSpeed(400f);
            SecondaryEffect.Modifiers.Add(SecondaryOM);

            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(750, 20);
            SecondaryEffect.Initialise(750, 20);

        }


        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);

            if (!Created[Created.Length - 1])
            {
                //Artificially keep the main spell alive so we can continue to spawn children
                /*if (!IsAlive)
                {
                    IsAlive = true;
                    Invisible = true;
                }*/

                CreateDelay += GameTime.ElapsedGameTime.Milliseconds;

                if (!Created[CurrentIndex] && CreateDelay >= CreateDelayMax)
                {
                    Created[CurrentIndex] = true;
                    CreateFollowShot();
                    CreateDelay -= CreateDelayMax;
                    CurrentIndex++;
                }

            }

        }

        private void CreateFollowShot()
        {
            LevelPTR.DamageEmitters.Add(
                       new DamageEmitter(
                       this.ParentEntity,
                       this.LevelPTR,
                        AdjustPositionToFront(this.SpawnPosition, this.Direction),
                       this.GetDirection(),
                       this.GetMaxDuration(),
                       this.GetRadius(),
                       SpawnSpeed,
                       this.GetDamage(),
                       this.IsMultiTarget(),
                       this.IsPlayerOwned(),
                       this.GetTexture(),
                       this.GetSkillType()));
            //Copy effects
            LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].MainEffect = (CircleEmitter)MainEffect.DeepCopy();
            LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].SecondaryEffect = (CircleEmitter)SecondaryEffect.DeepCopy();

            LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].MainEffect.Initialise(750, 20);
            LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].SecondaryEffect.Initialise(750, 20);

        }

        public override void Draw(SpriteBatch SpriteBatch, Level LevelPTR)
        {
            base.Draw(SpriteBatch, LevelPTR);
        }

    }
}
