using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace River
{
    //Wrapper for DamageEmitter class/extensions
    class Skill
    {
        //TODO: Add static texture2ds for attacks or idc

        static private Texture2D PrimaryAttackTexture;
        static private Texture2D[] SecondaryAttackTexture = new Texture2D[3];
        static private Texture2D[] DefensiveAttackTexture = new Texture2D[3];
        static private Texture2D[] SpecialAttackTexture = new Texture2D[3];

        private EntityType Class; //Used for loading class based art

        private Entity ParentEntity;
        private Level LevelPTR;

        #region Initialization/Loading

        public Skill(Level LevelPTR, EntityType Class, Entity ParentEntity)
        {
            this.LevelPTR = LevelPTR;
            this.Class = Class;
            this.ParentEntity = ParentEntity;
        }

        static public void LoadContent(ContentManager Content)
        {
            //TODO: load in any enemy attack sheets here.
        }

        static public void LoadConditionalContent(ContentManager Content, EntityType Type)
        {
            switch (Type)
            {
                case EntityType.Warrior:
                    PrimaryAttackTexture = Content.Load<Texture2D>(@"Textures\Attacks\Arcanebolt");
                    //PrimaryAttackTexture = null;
                    break;
                case EntityType.Bandit:
                    PrimaryAttackTexture = Content.Load<Texture2D>(@"Textures\Attacks\Attacks_Thief");
                    SecondaryAttackTexture[0] = Content.Load<Texture2D>(@"Textures\Attacks\Attacks_Thief");
                    SecondaryAttackTexture[1] = Content.Load<Texture2D>(@"Textures\Attacks\Attacks_Thief");
                    SecondaryAttackTexture[2] = Content.Load<Texture2D>(@"Textures\Attacks\Attacks_Thief");
                    DefensiveAttackTexture[0] = null;
                    DefensiveAttackTexture[1] = null;
                    DefensiveAttackTexture[2] = null;
                    SpecialAttackTexture[0] = Content.Load<Texture2D>(@"Textures\Attacks\Attacks_Thief");
                    SpecialAttackTexture[1] = Content.Load<Texture2D>(@"Textures\Attacks\Attacks_Thief");
                    SpecialAttackTexture[2] = Content.Load<Texture2D>(@"Textures\Attacks\Attacks_Thief");
                    break;
                case EntityType.Magician:
                    PrimaryAttackTexture = Content.Load<Texture2D>(@"Textures\Attacks\Arcanebolt");
                    SecondaryAttackTexture[0] = Content.Load<Texture2D>(@"Textures\Attacks\Frostbolt");
                    SecondaryAttackTexture[1] = Content.Load<Texture2D>(@"Textures\Attacks\Fireball");
                    SecondaryAttackTexture[2] = Content.Load<Texture2D>(@"Textures\Attacks\Lightningbolt");
                    DefensiveAttackTexture[0] = Content.Load<Texture2D>(@"Textures\Attacks\IceShield");
                    DefensiveAttackTexture[1] = null;
                    DefensiveAttackTexture[2] = Content.Load<Texture2D>(@"Textures\Attacks\LightningShield");
                    SpecialAttackTexture[0] = Content.Load<Texture2D>(@"Textures\Attacks\Lightningbolt");
                    SpecialAttackTexture[1] = Content.Load<Texture2D>(@"Textures\Attacks\Lightningbolt");
                    SpecialAttackTexture[2] = Content.Load<Texture2D>(@"Textures\Attacks\Lightningbolt");
                    break;
            }
        }

        #endregion

        #region Cast functions

        //Enemy cast function
        public void EnemyCast(Vector2 Position, Vector2 DirectionVector, string AnimationDir)
        {

        }

        //Player cast function
        public void PlayerCast(SkillType Skill, Vector2 Position, Vector2 DirectionVector)
        {
           
            switch (Skill)
            {
                #region Warrior
                case SkillType.Slash:
                    LevelPTR.DamageEmitters.Add(new Skills.Slash(ParentEntity, LevelPTR, Position, DirectionVector, PrimaryAttackTexture));
                    break;

                case SkillType.Strike:
                    LevelPTR.DamageEmitters.Add(new Skills.Strike(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[0]));
                    break;

                case SkillType.Cleave:
                    LevelPTR.DamageEmitters.Add(new Skills.Cleave(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[1]));
                    break;

                case SkillType.TeleportStrike:
                    LevelPTR.DamageEmitters.Add(new Skills.TeleportStrike(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[2]));
                    break;

                case SkillType.Whirlwind:
                    LevelPTR.DamageEmitters.Add(new Skills.WhirlWind(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[2]));
                    break;

                #endregion

                #region Bandit
                case SkillType.Arrow:
                    LevelPTR.DamageEmitters.Add(new Skills.Arrow(ParentEntity, LevelPTR, Position, DirectionVector, PrimaryAttackTexture));
                    break;
                case SkillType.PiercingShot:
                    LevelPTR.DamageEmitters.Add(new Skills.PiercingShot(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[0]));
                    break;

                case SkillType.Multishot:
                    LevelPTR.DamageEmitters.Add(new Skills.Multishot(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[1]));
                    break;

                case SkillType.FlamingArrow:
                    LevelPTR.DamageEmitters.Add(new Skills.FlamingArrow(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[2]));
                    break;

                case SkillType.Vanish:
                    LevelPTR.DamageEmitters.Add(new Skills.Vanish(ParentEntity, LevelPTR, Position, DirectionVector, DefensiveAttackTexture[0]));
                    break;

                case SkillType.Bandage:
                    LevelPTR.DamageEmitters.Add(new Skills.Bandage(ParentEntity, LevelPTR, Position, DirectionVector, DefensiveAttackTexture[1]));
                    break;

                case SkillType.Blind:
                    LevelPTR.DamageEmitters.Add(new Skills.Blind(ParentEntity, LevelPTR, Position, DirectionVector, DefensiveAttackTexture[2]));
                    break;

                case SkillType.PlagueArrow:
                    LevelPTR.DamageEmitters.Add(new Skills.PlagueArrow(ParentEntity, LevelPTR, Position, DirectionVector, SpecialAttackTexture[0]));
                    break;

                case SkillType.Haste:
                    LevelPTR.DamageEmitters.Add(new Skills.Haste(ParentEntity, LevelPTR, Position, DirectionVector, SpecialAttackTexture[1]));
                    break;

                case SkillType.AoeShot:
                    LevelPTR.DamageEmitters.Add(new Skills.AoeShot(ParentEntity, LevelPTR, Position, DirectionVector, SpecialAttackTexture[2]));
                    break;

                #endregion

                #region Magician
                case SkillType.Arcanebolt:
                    LevelPTR.DamageEmitters.Add(new Skills.Arcanebolt(ParentEntity, LevelPTR, Position, DirectionVector, PrimaryAttackTexture));
                    break;

                case SkillType.Frostbolt:
                    LevelPTR.DamageEmitters.Add(new Skills.Frostbolt(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[0]));
                    break;

                case SkillType.FireBall:
                    LevelPTR.DamageEmitters.Add(new Skills.Fireball(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[1]));
                    break;

                case SkillType.Lightningbolt:
                    LevelPTR.DamageEmitters.Add(new Skills.Lightningbolt(ParentEntity, LevelPTR, Position, DirectionVector, SecondaryAttackTexture[2]));
                    break;

                case SkillType.IceShield:
                    LevelPTR.DamageEmitters.Add(new Skills.IceShield(ParentEntity, LevelPTR, Position, DirectionVector, DefensiveAttackTexture[0]));
                    break;

                case SkillType.LightningShield:
                    LevelPTR.DamageEmitters.Add(new Skills.LightningShield(ParentEntity, LevelPTR, Position, DirectionVector, DefensiveAttackTexture[1]));
                    break;

                case SkillType.Cauterize:
                    LevelPTR.DamageEmitters.Add(new Skills.Cauterize(ParentEntity, LevelPTR, Position, DirectionVector, DefensiveAttackTexture[2]));
                    break;

                case SkillType.FrostExplosion:
                    LevelPTR.DamageEmitters.Add(new Skills.FrostExplosion(ParentEntity, LevelPTR, Position, DirectionVector, SpecialAttackTexture[0]));
                    break;

                case SkillType.FireWalk:
                    LevelPTR.DamageEmitters.Add(new Skills.FireWalk(ParentEntity, LevelPTR, Position, DirectionVector, SpecialAttackTexture[1]));
                    break;
                case SkillType.Storm:
                    LevelPTR.DamageEmitters.Add(new Skills.Storm(ParentEntity, LevelPTR, Position, DirectionVector, SpecialAttackTexture[2]));
                    break;

                #endregion
            }
        }

        #endregion


        //Get skill being cast
        public SkillType GetSkill(int SelectionID, CastButton CastButton)
        {
            SkillType SpellToCast = SkillType.None;

            #region Pick skill based on button & selection
            switch (Class)
            {
                case EntityType.Warrior:
                    switch (CastButton)
                    {
                        case CastButton.A:
                            SpellToCast = SkillType.Slash;
                            break;
                        case CastButton.X:
                            switch (SelectionID)
                            {
                                case 0:
                                    SpellToCast = SkillType.Strike;
                                    break;
                                case 1:
                                    SpellToCast = SkillType.Cleave;
                                    break;
                                case 2:
                                    SpellToCast = SkillType.TeleportStrike;
                                    break;
                            }
                            break;
                        case CastButton.B:
                            switch (SelectionID)
                            {
                                case 0:
                                    SpellToCast = SkillType.Shield;
                                    break;
                                case 1:
                                    SpellToCast = SkillType.Absorb;
                                    break;
                                case 2:
                                    SpellToCast = SkillType.Fear;
                                    break;
                            }
                            break;
                        case CastButton.Y:
                            switch (SelectionID)
                            {
                                case 0:
                                    SpellToCast = SkillType.Whirlwind;
                                    break;
                                case 1:
                                    SpellToCast = SkillType.Rage;
                                    break;
                                case 2:
                                    SpellToCast = SkillType.Crush;
                                    break;
                            }
                            break;
                    }
                    break;
                case EntityType.Magician:
                    switch (CastButton)
                    {
                        case CastButton.A:
                            SpellToCast = SkillType.Arcanebolt;
                            break;
                        case CastButton.X:
                            switch (SelectionID)
                            {
                                case 0:
                                    SpellToCast = SkillType.Frostbolt;
                                    break;
                                case 1:
                                    SpellToCast = SkillType.FireBall;
                                    break;
                                case 2:
                                    SpellToCast = SkillType.Lightningbolt;
                                    break;
                            }
                            break;
                        case CastButton.B:
                            switch (SelectionID)
                            {
                                case 0:
                                    SpellToCast = SkillType.IceShield;
                                    break;
                                case 1:
                                    SpellToCast = SkillType.Cauterize;
                                    break;
                                case 2:
                                    SpellToCast = SkillType.LightningShield;
                                    break;
                            }
                            break;
                        case CastButton.Y:
                            switch (SelectionID)
                            {
                                case 0:
                                    SpellToCast = SkillType.FrostExplosion;
                                    break;
                                case 1:
                                    SpellToCast = SkillType.FireWalk;
                                    break;
                                case 2:
                                    SpellToCast = SkillType.Storm;
                                    break;
                            }
                            break;
                    }

                    break;
                case EntityType.Bandit:
                    switch (CastButton)
                    {
                        case CastButton.A:
                            SpellToCast = SkillType.Arrow;
                            break;
                        case CastButton.X:
                            switch (SelectionID)
                            {
                                case 0:
                                    SpellToCast = SkillType.Multishot;
                                    break;
                                case 1:
                                    SpellToCast = SkillType.FlamingArrow;
                                    break;
                                case 2:
                                    SpellToCast = SkillType.PiercingShot;
                                    break;
                            }
                            break;
                        case CastButton.B:
                            switch (SelectionID)
                            {
                                case 0:
                                    SpellToCast = SkillType.Vanish;
                                    break;
                                case 1:
                                    SpellToCast = SkillType.Bandage;
                                    break;
                                case 2:
                                    SpellToCast = SkillType.Blind;
                                    break;
                            }
                            break;
                        case CastButton.Y:
                            switch (SelectionID)
                            {
                                case 0:
                                    SpellToCast = SkillType.PlagueArrow;
                                    break;
                                case 1:
                                    SpellToCast = SkillType.AoeShot;
                                    break;
                                case 2:
                                    SpellToCast = SkillType.Haste;
                                    break;
                            }
                            break;
                    }
                    break;
            }

            #endregion

            return SpellToCast;
        }

    }

    public enum CastButton
    {
        A,
        X,
        Y,
        B
    }

    public enum SkillType
    {
        None,
        //Mage
        Arcanebolt,
        Frostbolt,
        FireBall,
        Lightningbolt,
        IceShield,
        Cauterize,
        LightningShield,
        FrostExplosion,
        FireWalk,
        Storm,

        //Warrior
        Slash,
        Strike,
        Cleave,
        TeleportStrike,
        Shield,
        Absorb,
        Fear,
        Whirlwind,
        Rage,
        Crush,

        //Bandit
        Arrow,
        Multishot,
        FlamingArrow,
        PiercingShot,
        Vanish,
        Bandage,
        Blind,
        PlagueArrow,
        AoeShot,
        Haste,
    }
}
