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
    class Storm : DamageEmitter
    {
        private const int MaxChains = 4;
        private int ChainCounter;

        public Storm(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 500f,
            float Radius = 96f,
            float Speed = 3f,
            float Damage = 1f,
            bool MultiTarget = false,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.Storm,
            Buff Debuff = null,
            int ChainCounter = 0)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            this.ChainCounter = ChainCounter;

            ////////////
            //MAIN:
            ////////////
            MainEffect = new CircleEmitter();
            MainEffect.Radius = 16;
            MainEffect.ReleaseColour = Color.Blue.ToVector3();
            MainEffect.ReleaseQuantity = 24;
            MainEffect.ReleaseScale = new VariableFloat { Value = 24f, Variation = 8f };

            ControlledFade MainOM = new ControlledFade();
            MainOM.SetInitial(0.2f);
            MainOM.SetSpeed(250f);
            MainEffect.Modifiers.Add(MainOM);

            ////////////
            //SECONDARY:
            ////////////
            SecondaryEffect = new CircleEmitter();
            SecondaryEffect.Radius = 16;
            SecondaryEffect.ReleaseColour = Color.Yellow.ToVector3();
            SecondaryEffect.ReleaseQuantity = 24;
            SecondaryEffect.ReleaseScale = new VariableFloat { Value = 20f, Variation = 4f };

            ControlledFade SecondaryOM = new ControlledFade();
            SecondaryOM.SetInitial(0.2f);
            SecondaryOM.SetSpeed(250f);
            SecondaryEffect.Modifiers.Add(SecondaryOM);

            //Initialize
            MainEffect.ParticleTexture = ParticleTexture;
            MainEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEffect.ParticleTexture = ParticleTexture;
            SecondaryEffect.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            MainEffect.Initialise(250, 20);
            SecondaryEffect.Initialise(250, 20);

        }

        //Override the intersect function so we can teleport when the time is right
        public override bool Intersects(Vector2 ComparePosition, int Index)
        {
            bool Result = base.Intersects(ComparePosition, Index);

            if (Result == true)
            {

                if (ChainCounter >= MaxChains)
                    return Result;

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

                        NewPos = AdjustPositionToFront(
                            LevelPTR.Enemies[HitTargets[HitTargets.Count - 1]].Position,
                            DirectionVector);

                        LevelPTR.DamageEmitters.Add(
                            new Storm(
                            this.ParentEntity,
                            this.LevelPTR,
                            NewPos,
                            DirectionVector,
                            this.GetTexture(),
                            ChainCounter: ChainCounter + 1));

                        //Don't allow the hit target to be hit by the chain
                        LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].
                            HitTargets.Add(this.HitTargets[this.HitTargets.Count - 1]);
                    }
            }

            return Result;

        } //END INTERSECT

    } //End class

} //End namespace
