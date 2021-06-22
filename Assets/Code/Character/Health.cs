using UnityEngine;

namespace RPG.Character
{
    public class Health : MonoBehaviour
    {
        public float health = 100f;

        private Animator _animator;
        private CapsuleCollider _collider;

        public bool IsDead { get; private set; }

        public void Damage(float dmg)
        {
            if (IsDead) return;  //if he's already dead... dont need to keep damaging him

            health -= dmg;

            if (health < 0) health = 0;

            if (health == 0) Die();
        }

        void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider>();
        }

        private void Die()
        {
            _animator.SetTrigger("Die");
            IsDead = true;
            _collider.enabled = false;
        }
    }
}
