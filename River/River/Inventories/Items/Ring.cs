using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River.Items
{
    class Ring : Item
    {
        public Ring(int EnemyLevel, float MagicFind)
            : base(EnemyLevel, MagicFind,SlotType.Ring)
        {
          
        }

        public override void RandomizeStats(int EnemyLevel)
        {
            base.RandomizeStats(EnemyLevel);
            this.Armor = 0;
            this.Attack = 0;
            this.AttackSpeedBonus = 0;
        }

        protected override void SetIcon()
        {
            IconTexture = RingIcon[0];
        }
    }
}
