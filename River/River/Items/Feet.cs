using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace River.Items
{
    class Feet : Item
    {
        public Feet(int EnemyLevel, float MagicFind)
            : base(EnemyLevel, MagicFind, SlotType.Feet)
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
                int DisplayHaste = 0;


                if (this.Quality == QualityType.Blue)
                    DisplayHaste = Random.Next(-10, 10 + 1); //50% chance of a weak haste
                else if (this.Quality == QualityType.Yellow)
                    DisplayHaste = Random.Next(10, 20 + 1);
                else if (this.Quality == QualityType.Orange)
                    DisplayHaste = Random.Next(20, 45 + 1);

                if (DisplayHaste <= 0)
                    return;

                if (DisplayHaste > 30)
                    DisplayHaste = 30;

                //Adjust to a percentage (100% + x%)
                float BuffHaste = (float)DisplayHaste / 100f + 1f;

                this.Buff = new Buff("Movement Haste " + DisplayHaste.ToString() + "%", Buff.StateType.None, Buff.Indefinite, Buff.Indefinite, 0f, BuffHaste);
            }

        }

        protected override void SetIcon()
        {
            IconTexture = FeetIcon[0];
        }

    }
}
