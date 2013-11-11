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
    class LightningShield : DamageEmitter
    {
        public LightningShield(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 500f,
            float Radius = 192f,
            float Speed = 0f,
            float Damage = 3f,
            bool MultiTarget = true,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.LightningShield,
            Buff Debuff = null)

            :base(ParentEntity, LevelPTR,Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////
            MainEffect = new CircleEmitter();
            MainEffect.Radius = 192;
            MainEffect.ReleaseColour = Color.YellowGreen.ToVector3();
            MainEffect.ReleaseQuantity = 16;
            MainEffect.ReleaseScale = new VariableFloat { Value = 8f, Variation = 4f };
            MainEffect.Ring = true;

            ControlledFade MainOM = new ControlledFade();
            MainOM.SetInitial(0.5f);
            MainOM.SetSpeed(250f);
            MainEffect.Modifiers.Add(MainOM);

            ////////////
            //SECONDARY:
            ////////////
            SecondaryEffect = new CircleEmitter();
            SecondaryEffect.Radius = 192;
            SecondaryEffect.ReleaseColour = Color.Yellow.ToVector3();
            SecondaryEffect.ReleaseQuantity = 32;
            SecondaryEffect.ReleaseScale = new VariableFloat { Value = 12f, Variation = 4f };

            ControlledFade SecondaryOM = new ControlledFade();
            SecondaryOM.SetInitial(0.25f);
            SecondaryOM.SetSpeed(250f);
            SecondaryEffect.Modifiers.Add(SecondaryOM);

            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(750, 20);
            SecondaryEffect.Initialise(750, 20);
        }
    }
}
