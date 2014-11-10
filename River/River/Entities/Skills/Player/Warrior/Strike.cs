using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Emitters;
using ProjectMercury;

namespace River.Skills
{
    class Strike : DamageEmitter
    {
        public Strike(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 250f,
            float Radius = 96f,
            float Speed = 0f,
            float Damage = 1f,
            bool MultiTarget = false,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.Strike,
            Buff Debuff = null)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////

            SetEffect(ref MainEffect, 16f, Color.White, 24, new VariableFloat { Value = 16f, Variation = 4f }, 0.05f, 100f);

            ////////////
            //SECONDARY:
            ////////////

            SetEffect(ref SecondaryEffect, 2f, Color.Gray, 24, new VariableFloat { Value = 4f, Variation = 0f }, 0.5f, 300f);

            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(1000, 20);
            SecondaryEffect.Initialise(1000, 20);

            AdjustPositionToFront(ref this.Position, Direction);
        }

        private bool[] Created = new bool[5];
        private int CurrentIndex = 0;
        private float CreateDelayMax = 100f;
        private float CreateDelay = 0f;
        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);

            if (!Created[Created.Length - 1])
            {
                //Artificially keep the main spell alive so we can continue to spawn children
                if (!IsAlive)
                    IsAlive = true;

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
            if (HitTargets.Count == 0)
                return;

            LevelPTR.DamageEmitters.Add(
                       new DamageEmitter(
                       this.ParentEntity,
                       this.LevelPTR,
                        AdjustPositionToFront(this.SpawnPosition, this.Direction),
                       this.GetDirection(),
                       this.GetMaxDuration(),
                       this.GetRadius(),
                       this.GetSpeed(),
                       this.GetDamage(),
                       this.IsMultiTarget(),
                       this.IsPlayerOwned(),
                       this.GetTexture(),
                       this.GetSkillType()));

            //Claim that we have hit every target except the one that this has hit.
            //This will make it so that whatever enemy was struck first will literally be the only enemy we can hit again
            for (int ecx = 0; ecx < LevelPTR.Enemies.Length; ecx++)
            {
                if (ecx != HitTargets[0])
                    LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].HitTargets.Add(ecx);
            }

        }

    }
}
