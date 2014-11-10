using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace River.Items
{
    class Chest : Item
    {
        public Chest(int EnemyLevel, float MagicFind)
            : base(EnemyLevel, MagicFind, SlotType.Chest)
        {

        }

        public override void RandomizeStats(int EnemyLevel)
        {
            base.RandomizeStats(EnemyLevel);
            this.Attack = 0;
            this.AttackSpeedBonus = 0;
        }

        protected override void TryGiveBuff()
        {
            if (this.Quality != QualityType.White)
            {
                int Healing = 0;


                if (this.Quality == QualityType.Blue)
                    Healing = Random.Next(-10, 10 + 1); //50% chance of a moderate haste
                else if (this.Quality == QualityType.Yellow)
                    Healing = Random.Next(10, 25 + 1);
                else if (this.Quality == QualityType.Orange)
                    Healing = Random.Next(25, 45 + 1);

                if (Healing <= 0)
                    return;

                if (Healing > 35)
                    Healing = 35;

                this.Buff = new Buff("Recover " + Healing.ToString() + " / sec", Buff.StateType.None, Buff.Indefinite, 1000f, Healing, 1f);
            }
        }

        protected override void SetIcon()
        {
            IconTexture = ChestIcon[0];
        }
    }
}
