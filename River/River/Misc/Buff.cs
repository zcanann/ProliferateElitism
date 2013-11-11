using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River
{
    class Buff
    {
        public const float Indefinite = 0f;

        //Note: Organized by draw importance
        //Things that show up visually on the player
        public enum StateType
        {
            None,
            Blind,
            Poison,
            Burn,
            Chill,
            Freeze,
            Cloak, //Most important to draw
        }

        private StateType State = StateType.None;

        private string Name;
        private float DurationMax;
        private float DurationCurrent;
        private float TickDurationMax;
        private float TickDurationCurrent;
        private float TickHealthOffset;   //Damage or healing

        private float Speed; //1f = 100%
        private float SpeedMultiplier; // 0 = all movement nullified

        #region Constructors

        //DoT/HoT CONSTRUCTOR
        public Buff( string Name, StateType State, float DurationMax, float TickDurationMax, float TickHealthOffset, float SpeedMultiplier)
        {
            //Defaults -- buffs generally won't use these
            //this.Attack = 0;
            //this.Dexterity = 0;
            //this.Strength = 0;
            //this.Intellect = 0;
            //this.Vitality = 0;
            this.Speed = 0;

            this.Name = Name;
            this.State = State;
            this.TickDurationMax = TickDurationMax;
            this.DurationMax = DurationMax;
            this.DurationCurrent = DurationMax;
            this.TickHealthOffset = TickHealthOffset;
            this.SpeedMultiplier = SpeedMultiplier;
        }

        //Full constructor
        public Buff(string Name, float Speed, StateType State, float DurationMax, float TickDurationMax, float TickHealthOffset, float SpeedMultiplier)
        {
            /*this.Attack = Attack;
            this.Armor = Armor;
            this.Dexterity = Dexterity;
            this.Strength = Strength;
            this.Intellect = Intellect;
            this.Vitality = Vitality;*/
            this.Name = Name;
            this.Speed = Speed;
            this.State = State;
            this.DurationMax = DurationMax;
            this.DurationCurrent = DurationMax;
            this.TickDurationMax = TickDurationMax;
            this.TickHealthOffset = TickHealthOffset;
            this.SpeedMultiplier = SpeedMultiplier;
        }

        #endregion

        #region Getters

        /*
        public float GetAttack()
        {
            return Attack;
        }

        public int GetArmor()
        {
            return Armor;
        }

        public int GetDexterity()
        {
            return Dexterity;
        }

        public int GetStrength()
        {
            return Strength;
        }

        public int GetIntellect()
        {
            return Intellect;
        }

        public int GetVitality()
        {
            return Vitality;
        }*/
        public string GetName()
        {
            return Name;
        }

        public float GetSpeed()
        {
            return Speed;
        }

        public float GetSpeedMultiplier()
        {
            return SpeedMultiplier;
        }

        public float GetDurationMax()
        {
            return DurationMax;
        }
        public float GetDurationCurrent()
        {
            return DurationCurrent;
        }
        public float GetTickDurationMax()
        {
            return TickDurationMax;
        }
        public float GetTickDurationCurrent()
        {
            return TickDurationCurrent;
        }
        public float GetTickHealthOffset()
        {
            return TickHealthOffset;
        }

        public StateType GetState()
        {
            return State;
        }

        #endregion

        #region Public time functions

        public void ResetTickTimer()
        {
            TickDurationCurrent = TickDurationMax;
        }

        public void ReduceDuration(float Time)
        {
            DurationCurrent -= Time;
        }

        public void ReduceTickDurationCurrent(float Time)
        {
            TickDurationCurrent -= Time;
        }

        #endregion

    }
}
