using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury;
using ProjectMercury.Emitters;

namespace River.Skills
{
    class AoeShot : DamageEmitter
    {
        public AoeShot(
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
            SkillType SkillType = SkillType.AoeShot,
            Buff Debuff = null)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            CreateArrows();

            //Put leading skill in front (have to do this after since we use its position to calculate
            //the position of the other attacks)
            AdjustPositionToFront(ref this.Position, this.GetDirection());
        }


        private void CreateArrows()
        {
            ////////////
            //MAIN:
            ////////////

            //SetEffect(ref MainEffect, 8f, Color.Black, 8, new VariableFloat { Value = 24f, Variation = 0f }, 0.05f, 100f);
            SetEffect(ref MainEffect, 16f, Color.White, 8, new VariableFloat { Value = 16f, Variation = 4f }, 0.05f, 100f);

            ////////////
            //SECONDARY:
            ////////////

            SetEffect(ref SecondaryEffect, 2f, Color.Gray, 12, new VariableFloat { Value = 4f, Variation = 0f }, 0.5f, 300f);

            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(250, 3);

            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.Initialise(250, 3);

            Vector2 NewPos;
            Vector2 DirectionVector = new Vector2();

            for (int Y = -1; Y <= 1; Y++)
                for (int X = -1; X <= 1; X++)
                {
                    //Dont create one for no direction or in the same direction as main shot
                    if ((X == 0 && Y == 0) ||
                        (X == this.GetDirection().X && Y == this.GetDirection().Y))
                        continue;

                    DirectionVector.X = X;
                    DirectionVector.Y = Y;

                    NewPos = AdjustPositionToFront(this.SpawnPosition, DirectionVector);

                    LevelPTR.DamageEmitters.Add(
                        new DamageEmitter(
                        this.ParentEntity,
                        this.LevelPTR,
                        NewPos,
                        DirectionVector,
                        this.GetMaxDuration(),
                        this.GetRadius(),
                        this.GetSpeed(),
                        this.GetDamage(),
                        this.IsMultiTarget(),
                        this.IsPlayerOwned(),
                        this.GetTexture(),
                        this.GetSkillType()));

                    //Copy effects
                    LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].MainEffect = (EllipseEmitter)MainEffect.DeepCopy();
                    LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].MainEffect.Initialise(250, 5);

                    //CreateSideShots(this.LevelPTR, this.ParentEntity, LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1],
                      //  (float)Math.PI / 8f);
                }

            
        }

    }
}
