using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury;

namespace River.Skills.SubSkills
{
    class TeleportFinished : DamageEmitter
    {
        private const float EffectDuration = 500f;

        public TeleportFinished(Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = EffectDuration,
            float Radius = 0f,
            float Speed = 0f,
            float Damage = 0f,
            bool MultiTarget = false,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.None)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType)
        {
            ////////////
            //MAIN:
            ////////////

            SetEffect(ref MainEffect, 40f, Color.White, 24, new VariableFloat { Value = 16f, Variation = 4f }, 0.05f, 50f);

            ////////////
            //SECONDARY:
            ////////////

            SetEffect(ref SecondaryEffect, 40f, Color.Black, 24, new VariableFloat { Value = 4f, Variation = 0f }, 0.5f, 50f, true);


            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(1000, 20);
            SecondaryEffect.Initialise(1000, 20);
        }

        public override void Update(GameTime GameTime)
        {
            MainEffect.Radius += 0.05f * GameTime.ElapsedGameTime.Milliseconds;
            SecondaryEffect.Radius += 0.05f * GameTime.ElapsedGameTime.Milliseconds;
            base.Update(GameTime);
        }


        public override bool Intersects(Vector2 ComparePosition, int Index)
        {
            return false;
        }

    }
}
