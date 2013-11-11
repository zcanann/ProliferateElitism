﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;
using ProjectMercury;

namespace River.Skills
{
    class Arcanebolt : DamageEmitter
    {
        public Arcanebolt(
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
            SkillType SkillType = SkillType.Arcanebolt,
            Buff Debuff = null)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {

            ////////////
            //MAIN:
            ////////////
            MainEffect = new CircleEmitter();
            MainEffect.Radius = 16;
            MainEffect.ReleaseColour = Color.Purple.ToVector3();
            MainEffect.ReleaseQuantity = 24;
            MainEffect.ReleaseRotation = new VariableFloat { Value = 0f, Variation = 0.0f };
            MainEffect.ReleaseScale = new VariableFloat { Value = 16f, Variation = 4f };

            ControlledFade MainOM = new ControlledFade();
            MainOM.SetInitial(0.05f);
            MainOM.SetSpeed(100f);
            MainEffect.Modifiers.Add(MainOM);

            ////////////
            //SECONDARY:
            ////////////
            SecondaryEffect = new CircleEmitter();
            SecondaryEffect.Radius = 12;
            SecondaryEffect.ReleaseColour = Color.MediumPurple.ToVector3();
            SecondaryEffect.ReleaseQuantity = 64;
            SecondaryEffect.ReleaseRotation = new VariableFloat { Value = 0f, Variation = 0.0f };
            SecondaryEffect.ReleaseScale = new VariableFloat { Value = 12f, Variation = 2f };

            ControlledFade SecondaryOM = new ControlledFade();
            SecondaryOM.SetInitial(0.05f);
            SecondaryOM.SetSpeed(400f);
            SecondaryEffect.Modifiers.Add(SecondaryOM);
            
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
