using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace River.Skills
{
    class Bandage : DamageEmitter
    {
        public Bandage(
            Entity ParentEntity,
            Level LevelPTR,
            Vector2 Position,
            Vector2 Direction,
            Texture2D Texture,
            float Duration = 0f,
            float Radius = 0f,
            float Speed = 0f,
            float Damage = 0f,
            bool MultiTarget = false,
            bool PlayerOwned = false,
            SkillType SkillType = SkillType.Bandage,
            Buff Debuff = null)

            : base(ParentEntity, LevelPTR, Position, Direction, Duration, Radius, Speed, Damage, MultiTarget, PlayerOwned,
            Texture, SkillType, Debuff)
        {
            Buff Heal = new Buff("Bandage", Buff.StateType.None, 500f, 500f, 100, 1f);
            ParentEntity.ActiveBuffs.Add(Heal);
        }


    }
}
