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
    class Cleave : DamageEmitter
    {

        public Cleave(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 250f,
            float Radius = 96f,
            float Speed = 0f,
            float Damage = 1f,
            bool MultiTarget = true,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.Cleave,
            Buff Debuff = null)

            :base(ParentEntity, LevelPTR,Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////

            SetEffect(ref MainEffect, 48f, Color.White, 24, new VariableFloat { Value = 24f, Variation = 4f }, 0.05f, 100f);

            ////////////
            //SECONDARY:
            ////////////

            SetEffect(ref SecondaryEffect, 32f, Color.Violet, 12, new VariableFloat { Value = 16f, Variation = 0f }, 0.5f, 300f);

            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(1000, 20);
            SecondaryEffect.Initialise(1000, 20);

            AdjustPositionToFront(ref this.Position, Direction);
            CreateSideShots(this.LevelPTR, this.ParentEntity, this, (float)Math.PI / 4f);
        }

    }
}
