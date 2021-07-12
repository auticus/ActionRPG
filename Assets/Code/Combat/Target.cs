using System;
using RPG.Character;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class Target : MonoBehaviour
    {
        public enum TargetStatus
        {
            Alive = 0,
            Dead
        }

        public TargetStatus Status => _health.IsDead ? TargetStatus.Dead : TargetStatus.Alive;
        public bool IsPlayer => _isPlayer;

        public EventHandler onDeath;

        private Health _health;
        private BaseStats _stats;
        private bool _isPlayer;
        private bool _diedEventFired = false;

        void Start()
        {
            _health = GetComponent<Health>();
            _stats = GetComponent<BaseStats>();
            _isPlayer = this.CompareTag("Player");
        }

        public void Hit(int dmg)
        {
            _health.Damage(dmg);
            if (_health.IsDead && !_diedEventFired)
            {
                _diedEventFired = true;
                onDeath?.Invoke(this, EventArgs.Empty);
            }
        }

        public Health GetHealth() => _health;
        public int GetXp() => _stats.GetBaselineExperience();
    }
}
