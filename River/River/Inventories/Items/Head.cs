using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace River.Items
{
    class Head : Item
    {
        public Head(int EnemyLevel, float MagicFind)
            : base(EnemyLevel, MagicFind,SlotType.Head)
        {

        }

        public override void RandomizeStats(int EnemyLevel)
        {
            base.RandomizeStats(EnemyLevel);
            this.Attack = 0;
            this.AttackSpeedBonus = 0;
        }

        protected override void SetIcon()
        {
            IconTexture = HeadIcon[0];
        }

    }
}
