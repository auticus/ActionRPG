using System;
using System.Collections.Generic;
using Assets.Code.Combat;
using RPG.Character;
using RPG.Controllers;
using RPG.Interfaces;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
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
        private EnemyHealthDisplay _enemyHealthDisplay = null;
        private BaseStats _stats = null;
        private Experience _experience = null;

        void Start()
        {
            _movement = GetComponent<Movement>();
            _scheduler = GetComponent<Scheduler>();
            _animator = GetComponent<Animator>();

            _enemyHealthDisplay = GetComponent<EnemyHealthDisplay>();
            _stats = GetComponent<BaseStats>();
            _experience = GetComponent<Experience>();

            if (_currentWeapon == null) EquipWeapon(defaultWeapon); //otherwise saving system already did its job and we do nothing
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
            _target.onDeath += HandleDeath;

            if (gameObject.CompareTag("Player"))
            {
                _enemyHealthDisplay.SetTarget(target.GetHealth());
            }
        }

        public void Cancel()
        {
            _animator.ResetTrigger("DoAttack");
            _animator.SetTrigger("StopAttack");
            _target = null;

            if (gameObject.CompareTag("Player"))
            {
                _enemyHealthDisplay.SetTarget(null);
            }
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
            weapon.Spawn(rightHandTransform, leftHandTransform, animator, _stats);
        }

        public object CaptureState()
        {
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            var wpn = Resources.Load<Weapon>(state.ToString());
            EquipWeapon(wpn);
        }

        public IEnumerable<int> GetAdditiveModifiers(BaseStats.Stat stat)
        {
            if (stat == BaseStats.Stat.Damage)
            {
                yield return _currentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(BaseStats.Stat stat)
        {
            if (stat == BaseStats.Stat.Damage)
            {
                yield return _currentWeapon.GetPercentageModifier();
            }
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
            var damage = _stats.GetBaseDamage();

            if (_currentWeapon.IsRangedWeapon())
            {
                _currentWeapon.FireProjectile(rightHandTransform, leftHandTransform, _target, damage);
                return;
            }
            
            _target.Hit(damage);
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

        private void HandleDeath(object sender, EventArgs e)
        {
            //currently only the players can gain experience, but the hook exists for our enemies to get stronger as well
            if (_target == null)
            {
                Debug.LogWarning("Fighter::HandleDeath fires - target was null");
                return;
            }

            if (!_target.IsPlayer)
            {
                //player object caused this havoc, reward him some sweet sweet xp
                _experience.AddExperience(_target.GetXp());
            }
        }
    }
}