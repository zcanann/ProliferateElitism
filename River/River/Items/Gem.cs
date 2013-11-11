using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace River.Items
{
    class Gem : Item
    {
        public Gem(int EnemyLevel, float MagicFind, EntityType Type)
            : base(EnemyLevel, MagicFind, SlotType.None)
        {
            this.Quality = QualityType.White;
        }

        public override void RandomizeStats(int EnemyLevel)
        {
            //base.RandomizeStats(EnemyLevel);
            this.ItemLevel = 0;
            this.Attack = 0;
            this.AttackSpeedBonus = 0;
            this.Armor = 0;
            this.Dexterity = 0;
            this.Strength = 0;
            this.Intellect = 0;
            this.Vitality = 0;
        }

        protected override void SetIcon()
        {
            //TODO: use entity type to pick right icon
            IconTexture = GemIcon[0];
        }

    }
}
