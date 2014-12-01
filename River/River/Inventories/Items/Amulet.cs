using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River.Items
{
    class Amulet : Item
    {
        public Amulet(int Armor, int Primary, int Vitality, String Name, int Level, int Attack, float AttackSpeedBonus, int ItemID)
            : base(SlotType.Amulet, Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID)
        {

        }

        /*public override void RandomizeStats(int EnemyLevel)
        {
            base.RandomizeStats(EnemyLevel);
            this.Armor = 0;
            this.Attack = 0;
            this.AttackSpeedBonus = 0;
        }*/

        protected override void SetIcon()
        {
            IconTexture = AmuletIcon[0];
        }

    }
}
