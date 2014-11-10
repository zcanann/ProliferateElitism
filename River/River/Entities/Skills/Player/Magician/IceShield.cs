using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;

namespace River.Skills
{
    class IceShield : DamageEmitter
    {
        public IceShield(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 5000f,
            float Radius = 96f,
            float Speed = 0f,
            float Damage = 1f,
            bool MultiTarget = true,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.IceShield,
            Buff Debuff = null)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////

            SetEffect(ref MainEffect, 64, Color.YellowGreen, 24, new VariableFloat { Value = 24f, Variation = 4f }, 0.25f, 50f, true);
            MainEffect.ReleaseOpacity = new VariableFloat { Value = 0.5f, Variation = 0f };

            ////////////
            //SECONDARY:
            ////////////

            SetEffect(ref SecondaryEffect, 64f, Color.Aqua, 24, new VariableFloat { Value = 12f, Variation = 4f }, 0.25f, 50f, true);
            SecondaryEffect.ReleaseOpacity = new VariableFloat { Value = 0.5f, Variation = 0f };

            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(200, 20);
            SecondaryEffect.Initialise(200, 20);
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            LatchToParent(ref Position, ParentEntity.Position);
        }

    }
}
