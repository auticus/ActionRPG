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

        private Health _health;

        void Start()
        {
            _health = GetComponent<Health>();
        }

        public void Hit(float dmg)
        {
            _health.Damage(dmg);
        }
    }
}
