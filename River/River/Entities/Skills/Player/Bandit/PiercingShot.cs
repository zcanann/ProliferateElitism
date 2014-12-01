using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury;

namespace River.Skills
{
    class PiercingShot : DamageEmitter
    {
        public PiercingShot(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 500f,
            float Radius = 96f,
            float Speed = 4f,
            float Damage = 1f,
            bool MultiTarget = true,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.PiercingShot,
            Buff Debuff = null)

            : base(ParentEntity, LevelPTR,Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////

            SetEffect(ref MainEffect, 16f, Color.White, 24, new VariableFloat { Value = 16f, Variation = 4f }, 0.05f, 100f);

            ////////////
            //SECONDARY:
            ////////////

            SetEffect(ref SecondaryEffect, 16f, Color.White, 24, new VariableFloat { Value = 16f, Variation = 0f }, 0.5f, 100f);


            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(1000, 20);
            SecondaryEffect.Initialise(1000, 20);
        }
    }
}
