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
    class FlamingArrow : DamageEmitter
    {
        public FlamingArrow(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 500f,
            float Radius = 96f,
            float Speed = 4f,
            float Damage = 1f,
            bool MultiTarget = false,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.FlamingArrow)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, 
            
            new Buff("Arrow Burn", Buff.StateType.Burn, 4000f, 500f, -1f, 0.5f))
        {
            ////////////
            //MAIN:
            ////////////

            SetEffect(ref MainEffect, 20f, Color.Orange, 12, new VariableFloat { Value = 24f, Variation = 4f }, 0.1f, 100f);

            ////////////
            //SECONDARY:
            ////////////

            SetEffect(ref SecondaryEffect, 2f, Color.Orange, 4, new VariableFloat { Value = 4f, Variation = 0f }, 0.5f, 300f);


            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(500, 20);
            SecondaryEffect.Initialise(500, 20);
        }
    }
}
