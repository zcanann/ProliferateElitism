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
    class PlagueArrow : DamageEmitter
    {
        public PlagueArrow(
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
            SkillType SkillType = SkillType.PlagueArrow,
            Buff Debuff = null)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////

            SetEffect(ref MainEffect, 16f, Color.Green, 8, new VariableFloat { Value = 24f, Variation = 4f }, 0.1f, 100f);

            ////////////
            //SECONDARY:
            ////////////

            SetEffect(ref SecondaryEffect, 2f, Color.Black, 4, new VariableFloat { Value = 3f, Variation = 4f }, 0.5f, 300f);


            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(1000, 20);
            SecondaryEffect.Initialise(1000, 20);
        }

        public override bool Intersects(Vector2 ComparePosition, int Index)
        {
            bool Result = base.Intersects(ComparePosition, Index);

            if (Result == true)
            {
                //Create a plague emitter on hit target
                LevelPTR.DamageEmitters.Add(
                    new Skills.SubSkills.Plague(
                    this.ParentEntity,
                    this.LevelPTR,
                    this.Position,
                    this.GetDirection(),
                    null,
                    LastHitTargets: this.HitTargets));
            }

            return Result;
        }

    }
}
