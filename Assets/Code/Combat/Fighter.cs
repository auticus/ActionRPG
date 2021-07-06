using RPG.Character;
using RPG.Controllers;
using RPG.Interfaces;
using UnityEditor;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private float timeBetweenAttacks = 1f;

        private Target _target;
        private Movement _movement;
        private Scheduler _scheduler;
        private Animator _animator;
        private float _timeSinceLastAttack = Mathf.Infinity; //infinity so that its been infinity since our last attack
        private Weapon _currentWeapon = null;

        void Start()
        {
            _movement = GetComponent<Movement>();
            _scheduler = GetComponent<Scheduler>();
            _animator = GetComponent<Animator>();
            EquipWeapon(defaultWeapon);
        }

        void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null)
            {
                return;
            }

            if (!IsInRange())
            {
                _movement.MoveTo(_target.transform.position);
            }
            else
            {
                _movement.Cancel();
                AttackBehavior();
            }
        }

        public void Attack(Target target)
        {
            _scheduler.StartAction(this);
            _target = target;
        }

        public void Cancel()
        {
            _animator.ResetTrigger("DoAttack");
            _animator.SetTrigger("StopAttack");
            _target = null;
            GetComponent<Movement>().Cancel(); //if you fail to cancel here then canceling fighting does not cancel it moving to fight if it was already doing so!
        }

        public bool IsValidTarget(Target target)
        {
            return target != null && target.Status != Target.TargetStatus.Dead;
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            var animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.GetRange();
        }

        //this is an animation event called by the animator
        private void Hit()
        {
            AnimationAction();
        }

        //this animation is called by the bow animator
        private void Shoot()
        {
            AnimationAction();
        }

        private void AnimationAction()
        {
            if (_target == null) return;
            if (_currentWeapon.IsRangedWeapon())
            {
                var health = _target.GetHealth();
                _currentWeapon.FireProjectile(rightHandTransform, leftHandTransform, _target.GetHealth());
                return;
            }

            _target.Hit(_currentWeapon.GetDamage());
        }

        private void AttackBehavior()
        {
            if (_target.Status == Target.TargetStatus.Dead)
            {
                Cancel();
                return;
            }

            TurnToFaceTarget();
            if (_timeSinceLastAttack < timeBetweenAttacks) return;
            _animator.ResetTrigger("StopAttack");
            _animator.SetTrigger("DoAttack");
            _timeSinceLastAttack = 0;
        }

        private void TurnToFaceTarget()
        {
            transform.LookAt(_target.transform);
        }
    }
}