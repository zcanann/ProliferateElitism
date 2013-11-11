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
            MainEffect = new CircleEmitter();
            MainEffect.Radius = 64;
            MainEffect.ReleaseColour = Color.Blue.ToVector3();
            MainEffect.ReleaseQuantity = 24;
            MainEffect.ReleaseRotation = new VariableFloat { Value = 0f, Variation = 0.0f };
            MainEffect.ReleaseScale = new VariableFloat { Value = 24f, Variation = 4f };
            MainEffect.Ring = true;
            MainEffect.ReleaseOpacity = new VariableFloat { Value = 0.5f, Variation = 0f };

            ControlledFade MainOM = new ControlledFade();
            MainOM.SetInitial(0.25f);
            MainOM.SetSpeed(50f);
            MainEffect.Modifiers.Add(MainOM);

            ////////////
            //SECONDARY:
            ////////////
            SecondaryEffect = new CircleEmitter();
            SecondaryEffect.Radius = 64;
            SecondaryEffect.ReleaseColour = Color.Aqua.ToVector3();
            SecondaryEffect.ReleaseQuantity = 24;
            SecondaryEffect.ReleaseRotation = new VariableFloat { Value = 0f, Variation = 0.0f };
            SecondaryEffect.ReleaseScale = new VariableFloat { Value = 12f, Variation = 4f };
            SecondaryEffect.ReleaseOpacity = new VariableFloat { Value = 0.5f, Variation = 0f };
            SecondaryEffect.Ring = true;

            ControlledFade SecondaryOM = new ControlledFade();
            SecondaryOM.SetInitial(0.25f);
            SecondaryOM.SetSpeed(50f);
            SecondaryEffect.Modifiers.Add(SecondaryOM);


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
