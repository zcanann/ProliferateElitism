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
    class LightningShield : DamageEmitter
    {
        public LightningShield(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 500f,
            float Radius = 192f,
            float Speed = 0f,
            float Damage = 3f,
            bool MultiTarget = true,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.LightningShield,
            Buff Debuff = null)

            :base(ParentEntity, LevelPTR,Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////

            SetEffect(ref MainEffect, 192, Color.YellowGreen, 16, new VariableFloat { Value = 8f, Variation = 4f }, 0.5f, 250f, true);

            ////////////
            //SECONDARY:
            ////////////

            SetEffect(ref SecondaryEffect, 192, Color.Yellow, 32, new VariableFloat { Value = 12f, Variation = 4f }, 0.25f, 250f);

            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(750, 20);
            SecondaryEffect.Initialise(750, 20);
        }
    }
}
