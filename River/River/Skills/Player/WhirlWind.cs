using System;
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
    class WhirlWind : DamageEmitter
    {
        public WhirlWind(
             Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 250f,
            float Radius = 192f,
            float Speed = 0f,
            float Damage = 4f,
            bool MultiTarget = true,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.Arcanebolt,
            Buff Debuff = null,
            bool Initial = true)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            ////////////
            //MAIN:
            ////////////
            MainEffect = new CircleEmitter();
            MainEffect.Radius = 192;
            MainEffect.ReleaseColour = Color.DarkGray.ToVector3();
            MainEffect.ReleaseQuantity = 24;
            MainEffect.ReleaseScale = new VariableFloat { Value = 24f, Variation = 8f };
            MainEffect.Ring = true;

            ControlledFade MainOM = new ControlledFade();
            MainOM.SetInitial(0.2f);
            MainOM.SetSpeed(250f);
            MainEffect.Modifiers.Add(MainOM);

            ////////////
            //SECONDARY:
            ////////////
            SecondaryEffect = new CircleEmitter();
            SecondaryEffect.Radius = 192;
            SecondaryEffect.ReleaseColour = Color.DarkGray.ToVector3();
            SecondaryEffect.ReleaseQuantity = 32;
            SecondaryEffect.ReleaseScale = new VariableFloat { Value = 16f, Variation = 4f };

            ControlledFade SecondaryOM = new ControlledFade();
            SecondaryOM.SetInitial(0.2f);
            SecondaryOM.SetSpeed(250f);
            SecondaryEffect.Modifiers.Add(SecondaryOM);

            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(750, 20);
            SecondaryEffect.Initialise(750, 20);

            //if (Initial)
            //    CreateWhirlWindDEPRECATED();
        }

        //OLD
        private void CreateWhirlWindDEPRECATED()
        {
            Vector2 NewPos;
            Vector2 DirectionVector = new Vector2();

            for (int Y = -1; Y <= 1; Y++)
                for (int X = -1; X <= 1; X++)
                {
                    //Dont create one for no direction or in the same direction as main shot
                    if ((X == 0 && Y == 0))// ||
                        //(X == this.GetDirection().X && Y == this.GetDirection().Y))
                        continue;

                    DirectionVector.X = X;
                    DirectionVector.Y = Y;

                    NewPos = AdjustPositionToFront(Position, DirectionVector);
                    AdjustPositionToFront(ref NewPos, DirectionVector);

                    LevelPTR.DamageEmitters.Add(
                        new WhirlWind(
                        this.ParentEntity,
                        this.LevelPTR,
                        NewPos,
                        DirectionVector,
                        this.GetTexture(),
                        Initial: false));

                    //Don't allow the hit target to be hit by the chain
                    //LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].
                    //HitTargets.Add(this.HitTargets[this.HitTargets.Count - 1]);
                }
        }

    }
}