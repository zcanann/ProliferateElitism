using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River.Items
{
    class Amulet : Item
    {
        public Amulet(int EnemyLevel, float MagicFind)
            : base(EnemyLevel, MagicFind, SlotType.Amulet)
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
            IconTexture = AmuletIcon[0];
        }

    }
}
