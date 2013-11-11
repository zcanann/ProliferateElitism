﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury;
using ProjectMercury.Emitters;

namespace River.Skills
{
    class TeleportStrike : DamageEmitter
    {
        public TeleportStrike(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 500f,
            float Radius = 96f,
            float Speed = 4f,
            float Damage = 0.75f,
            bool MultiTarget = false,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.TeleportStrike,
            Buff Debuff = null)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////
            MainEffect = new CircleEmitter();
            MainEffect.Radius = 40;
            MainEffect.ReleaseColour = Color.White.ToVector3();
            MainEffect.ReleaseQuantity = 24;
            MainEffect.ReleaseScale = new VariableFloat { Value = 16f, Variation = 4f };

            ControlledFade MainOM = new ControlledFade();
            MainOM.SetInitial(0.05f);
            MainOM.SetSpeed(100f);
            MainEffect.Modifiers.Add(MainOM);

            ////////////
            //SECONDARY:
            ////////////
            
            SecondaryEffect = new CircleEmitter();
            SecondaryEffect.Radius = 1;
            SecondaryEffect.ReleaseColour = Color.Gray.ToVector3();
            SecondaryEffect.ReleaseQuantity = 24;
            SecondaryEffect.ReleaseScale = new VariableFloat { Value = 4f, Variation = 0f };

            ControlledFade SecondaryOM = new ControlledFade();
            SecondaryOM.SetInitial(0.5f);
            SecondaryOM.SetSpeed(300f);
            SecondaryEffect.Modifiers.Add(SecondaryOM);


            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(1000, 20);
            SecondaryEffect.Initialise(1000, 20);
        }

        //Override the intersect function so we can teleport to what was hit
        public override bool Intersects(Vector2 ComparePosition, int Index)
        {
            bool Result = base.Intersects(ComparePosition, Index);

            if (Result == true)
            {
                float Distance;
                Tile.CircleTest(ParentEntity.Position, Position, this.Radius, out Distance);
                if (Distance > 128f)
                {
                    ParentEntity.Position = ComparePosition;
                    ParentEntity.SpriteAnimation.Position = ComparePosition;
                }
            }

            return Result;
        }

    }
}
