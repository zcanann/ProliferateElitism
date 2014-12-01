using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace River.Items
{
    class Weapon : Item
    {
        public Weapon(int Armor, int Primary, int Vitality, String Name, int Level, int Attack, float AttackSpeedBonus, int ItemID)
            : base(SlotType.Weapon, Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID)
        {

        }

        /*public override void RandomizeStats(int EnemyLevel)
        {
            base.RandomizeStats(EnemyLevel);
            this.Armor = 0;
        }

        protected override void TryGiveBuff()
        {
            if (this.Quality != QualityType.White)
            {
                float BuffDamage = 0;
                float Duration = 0f;
                float TickDuration = 0f;
                float SpeedMultiplier = 1f;

                string BuffText = "";

                if (this.Quality == QualityType.Blue)
                {
                    //40% chance to enchant
                    if (Random.Next(0, 100) >= 40)
                        return;

                    BuffText = "Lesser ";
                    BuffDamage = .33f;
                }
                else if (this.Quality == QualityType.Yellow)
                {
                    //70% chance to enchant
                    if (Random.Next(0, 100) >= 70)
                        return;

                    //BuffText = "Major ";
                    BuffDamage = .66f;
                }
                else if (this.Quality == QualityType.Orange)
                {
                    BuffText = "Greater ";

                    BuffDamage = 1f; //100% weapon damage
                }

                //Random buff

            Retry:
                Buff.StateType BuffType = (Buff.StateType)(Random.Next(0, Enum.GetValues(typeof(Buff.StateType)).Length));
                switch (BuffType)
                {
                    case Buff.StateType.Burn:
                        Duration = 1000f;
                        TickDuration = 500f;
                        break;
                    case Buff.StateType.Chill:
                        SpeedMultiplier = 0.5f;
                        Duration = 2000f;
                        TickDuration = 1000f;
                        break;
                    case Buff.StateType.Freeze:
                        //Using the "damage" to set a percentage of the standard freeze time
                        Duration = 3000f * BuffDamage;
                        //Kill damage (freeze doesn't do damage)
                        BuffDamage = 0f;
                        SpeedMultiplier = 0f;
                        TickDuration = 0f;
                        break;
                    case Buff.StateType.Poison:
                        Duration = 1000f;
                        TickDuration = 500f;
                        break;
                    default:
                        goto Retry;
                }


                BuffText += BuffType.ToString() + " ";

                this.DebuffOnAttack = new Buff(BuffText, BuffType, Duration, TickDuration, 0f, SpeedMultiplier);
            }

        }*/

        protected override void SetIcon()
        {
            IconTexture = WeaponIcon[0];
        }

    }
}
