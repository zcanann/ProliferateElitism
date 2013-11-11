using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Emitters;
using ProjectMercury;
using ProjectMercury.Modifiers;

namespace River.Skills
{
    class Lightningbolt : DamageEmitter
    {
        private int MaxChains = 2;
        private int ChainCounter;
        private const float ChainDistance = 480f;

        public Lightningbolt(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 500f,
            float Radius = 96f,
            float Speed = 4.5f,
            float Damage = 1f,
            bool MultiTarget = false,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.Lightningbolt,
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
            MainEffect.Initialise(750, 20);
            SecondaryEffect.Initialise(750, 20);
        }

        public override bool Intersects(Vector2 ComparePosition, int Index)
        {
            bool Result = base.Intersects(ComparePosition, Index);

            //Check if still chaining & intersect was true
            if (Result == true)
            {

                if (ChainCounter >= MaxChains)
                    return Result;

                //Look for an enemy to chain lightning to
                int ecx = 0;
            Next:
                for (; ecx < LevelPTR.Enemies.Length; ecx++)
                {
                    //Dont fork to the same enemy ever
                    for (int edi = 0; edi < HitTargets.Count; edi++)
                        if (ecx == HitTargets[edi] || !LevelPTR.Enemies[ecx].IsAlive)
                        {
                            ecx++;
                            goto Next;
                        }

                    if (Tile.IntersectionTest(LevelPTR.Enemies[ecx].Position, this.Position, ChainDistance))
                    {
                        Vector2 MoveDir = UnitCircle.ComputeAngle(Position, LevelPTR.Enemies[ecx].Position);

                        //Movement takes into account aspect ratio -- we have to undo what it will do to preserve our angle
                        MoveDir.X /= 2;
                        LevelPTR.DamageEmitters.Add(
                            new Lightningbolt(
                            this.ParentEntity,
                            this.LevelPTR,
                            //this.Position,
                            AdjustPositionToFront(this.Position, MoveDir),
                            MoveDir,
                            this.GetTexture(),
                            ChainCounter: ChainCounter + 1));

                        //Migrate hit targets (so there is no re-infection)
                        LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].HitTargets = this.HitTargets;

                        return Result;
                    }

                }
            }

            return Result;
        }

    }
}
