using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace River.Items
{
    class Legs : Item
    {
        public Legs(int Armor, int Primary, int Vitality, String Name, int Level, int Attack, float AttackSpeedBonus)
            : base(SlotType.Legs, Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus)
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
            IconTexture = LegsIcon[0];
        }

    }
}
