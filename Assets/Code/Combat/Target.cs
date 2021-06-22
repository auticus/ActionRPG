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

        private Health _health;
        private bool _isPlayer;

        void Start()
        {
            _health = GetComponent<Health>();
            _isPlayer = this.CompareTag("Player");
        }

        public void Hit(float dmg)
        {
            _health.Damage(dmg);
        }
    }
}
