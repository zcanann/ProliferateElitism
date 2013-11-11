using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River.Items
{
    class Offhand : Item
    {
        public Offhand(int EnemyLevel, float MagicFind)
            : base(EnemyLevel, MagicFind, SlotType.Offhand)
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
            IconTexture = OffhandIcon[0];
        }
    }
}
