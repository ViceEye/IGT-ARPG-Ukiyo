using System;
using UnityEngine;

namespace Ukiyo.Serializable.Entity
{
    [Serializable]
    public class EntityStat : IEntityStat
    {
        [SerializeField]
        protected double _health;
        public double Health
        {
            get => _health;
            set => _health = value;
        }

        [SerializeField]
        protected double _maxHealth;
        public double MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        [SerializeField]
        protected double _damage;
        public double Damage
        {
            get => _damage;
            set => _damage = value;
        }

        [SerializeField]
        protected double _mana;
        public double Mana
        {
            get => _mana;
            set => _mana = value;
        }
        
        [SerializeField]
        protected double _maxMana;
        public double MaxMana
        {
            get => _maxMana;
            set => _maxMana = value;
        }

        [SerializeField]
        protected double _criticalRate;
        public double CriticalRate
        {
            get => _criticalRate;
            set => _criticalRate = value;
        }

        [SerializeField]
        protected double _criticalDamage;
        public double CriticalDamage
        {
            get => _criticalDamage;
            set => _criticalDamage = value;
        }

        [SerializeField]
        protected int _exp;
        public int Exp
        {
            get => _exp;
            set => _exp = value;
        }

        public EntityStat()
        {
        }

        public EntityStat(EntityStat copy)
        {
            Health = copy.Health;
            MaxHealth = copy.MaxHealth;
            Damage = copy.Damage;
            Mana = copy.Mana;
            MaxMana = copy.MaxMana;
            CriticalRate = copy.CriticalRate;
            CriticalDamage = copy.CriticalDamage;
            Exp = copy.Exp;
        }

        public override string ToString()
        {
            return $"{nameof(Health)}: {Health}," +
                   $" {nameof(MaxHealth)}: {MaxHealth}," +
                   $" {nameof(Damage)}: {Damage}," +
                   $" {nameof(Mana)}: {Mana}," +
                   $" {nameof(MaxMana)}: {MaxMana}," +
                   $" {nameof(CriticalRate)}: {CriticalRate}," +
                   $" {nameof(CriticalDamage)}: {CriticalDamage}," +
                   $" {nameof(Exp)}: {Exp}";
        }

        public EntityStat Clone()
        {
            return new EntityStat(this);
        }
    }
}