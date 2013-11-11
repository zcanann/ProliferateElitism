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
    class Slash : DamageEmitter
    {

        public Slash(
           Entity ParentEntity,
           Level LevelPTR,
           Vector2 Position,
           Vector2 Direction,
           Texture2D Texture,
           float Duration = 250f,
           float Radius = 96f,
           float Speed = 0f,
           float Damage = 1f,
           bool MultiTarget = true,
           bool PlayerOwned = true,
           SkillType SkillType = SkillType.Slash,
           Buff Debuff = null)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////
            MainEffect = new CircleEmitter();
            MainEffect.Radius = 16;
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

            AdjustPositionToFront(ref this.Position, Direction);
        }
    }
}
