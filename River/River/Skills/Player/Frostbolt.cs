﻿using System;
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
    class Frostbolt : DamageEmitter
    {
        public Frostbolt(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 500f,
            float Radius = 96f,
            float Speed = 5f,
            float Damage = 0.5f,
            bool MultiTarget = false,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.Frostbolt)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, 
            
            new Buff("Frostbolt Chill", Buff.StateType.Chill, 4000f, 500f, -1f, 0.5f))
        {
            ////////////
            //MAIN:
            ////////////
            MainEffect = new CircleEmitter();
            MainEffect.Radius = 16;
            MainEffect.ReleaseColour = Color.Aqua.ToVector3();
            MainEffect.ReleaseQuantity = 16;
            MainEffect.ReleaseScale = new VariableFloat { Value = 16f, Variation = 4f };

            ControlledFade MainOM = new ControlledFade();
            MainOM.SetInitial(0.2f);
            MainOM.SetSpeed(400f);
            MainEffect.Modifiers.Add(MainOM);

            LinearGravityModifier MainGM = new LinearGravityModifier();
            MainGM.Gravity = new Vector2(0, -256);
            MainEffect.Modifiers.Add(MainGM);

            ////////////
            //SECONDARY:
            ////////////
            SecondaryEffect = new CircleEmitter();
            SecondaryEffect.Radius = 12;
            SecondaryEffect.ReleaseColour = Color.Blue.ToVector3();
            SecondaryEffect.ReleaseQuantity = 16;
            SecondaryEffect.ReleaseScale = new VariableFloat { Value = 32f, Variation = 4f };

            ControlledFade SecondaryOM = new ControlledFade();
            SecondaryOM.SetInitial(0.05f);
            SecondaryOM.SetSpeed(400f);
            SecondaryEffect.Modifiers.Add(SecondaryOM);

            LinearGravityModifier SecondaryGM = new LinearGravityModifier();
            SecondaryGM.Gravity = new Vector2(0, -256);
            SecondaryEffect.Modifiers.Add(SecondaryGM);


            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(750, 20);
            SecondaryEffect.Initialise(750, 20);

            CreateSideShots(this.LevelPTR, this.ParentEntity, this, (float)Math.PI / 24f);
            AdjustPositionToFront(ref this.Position, Direction);

        }
    }
}
