using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River.Items
{
    class Offhand : Item
    {
        public Offhand(int Armor, int Primary, int Vitality, String Name, int Level, int Attack, float AttackSpeedBonus)
            : base(SlotType.Offhand, Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus)
        {

        }

        /*public override void RandomizeStats(int EnemyLevel)
        {
            base.RandomizeStats(EnemyLevel);
            this.Attack = 0;
            this.AttackSpeedBonus = 0;
        }*/

        protected override void SetIcon()
        {
            IconTexture = OffhandIcon[0];
        }
    }
}
