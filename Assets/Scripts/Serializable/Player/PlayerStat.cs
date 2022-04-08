using System;
using UnityEngine;

namespace Ukiyo.Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "Ukiyo/New Player Data")]
    public class PlayerStat : ScriptableObject, IPlayerStat
    {
        public float __health;
        [NonSerialized] protected float _health;
        public float Health { get => _health; set => _health = value; }

        public float __damage;
        [NonSerialized] protected float _damage;
        public float Damage { get => _damage; set => _damage = value; }

        public float __mana;
        [NonSerialized] protected float _mana;
        public float Mana { get => _mana; set => _mana = value; }

        public float __criticalRate;
        [NonSerialized] protected float _criticalRate;
        public float CriticalRate { get => _criticalRate; set => _criticalRate = value; }

        public float __criticalDamage;
        [NonSerialized] protected float _criticalDamage;
        public float CriticalDamage { get => _criticalDamage; set => _criticalDamage = value; }

        public int __exp;
        [NonSerialized] protected int _exp;
        public int Exp { get => _exp; set => _exp = value; }
    }
}
