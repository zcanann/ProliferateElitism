using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace River.Items
{
    class Hands : Item
    {
        public Hands(int EnemyLevel, float MagicFind)
            : base(EnemyLevel, MagicFind, SlotType.Hands)
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
            IconTexture = HandsIcon[0];
        }
    }
}
