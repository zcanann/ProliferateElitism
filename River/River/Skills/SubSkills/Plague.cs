using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace River.Skills.SubSkills
{
    class Plague : DamageEmitter
    {
        private int LatchEnemyIndex = -1;
        private const float PlagueDuration = 5000f;

        public Plague(Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            List<int> LastHitTargets,
            float Duration = PlagueDuration,
            float Radius = 480f,
            float Speed = 0f,
            float Damage = 0f,
            bool MultiTarget = true,
            bool PlayerOwned = true,
            SkillType SkillType = SkillType.None)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, new Buff("Plague", Buff.StateType.Poison, PlagueDuration, 500f, -1f, 1f))
        {
            LatchEnemyIndex = GetLatchTarget(ref LastHitTargets);
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);

            LatchToTarget(this.LevelPTR, ref this.Position, LatchEnemyIndex);
        }


        public override bool Intersects(Vector2 ComparePosition, int Index)
        {
            bool Result = base.Intersects(ComparePosition, Index);

            if (Result == true)
            {
                //Emitter that 'transmits' the plague debuff
                LevelPTR.DamageEmitters.Add(
                    new Skills.SubSkills.Plague(
                    this.ParentEntity,
                    this.LevelPTR,
                    this.Position,
                    this.GetDirection(),
                    null,
                    LastHitTargets: this.HitTargets));

                //Migrate hit targets (so there is no re-infection)
                LevelPTR.DamageEmitters[LevelPTR.DamageEmitters.Count - 1].HitTargets = this.HitTargets;
            }
            return Result;
        }

    }
}
