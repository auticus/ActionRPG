using RPG.Character;
using RPG.Controllers;
using RPG.Interfaces;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        
        private Target _target;
        private Movement _movement;
        private Scheduler _scheduler;
        private Animator _animator;
        private float _timeSinceLastAttack = 0;
        
        void Start()
        {
            _movement = GetComponent<Movement>();
            _scheduler = GetComponent<Scheduler>();
            _animator = GetComponent<Animator>();
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
        }

        public bool IsValidTarget(Target target)
        {
            return target != null && target.Status != Target.TargetStatus.Dead;
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < weaponRange;
        }

        //this is an animation event called by the animator
        private void Hit()
        {
            if (_target == null) return;
            _target.Hit(weaponDamage);
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
            Debug.Log("Maim!!");
        }

        private void TurnToFaceTarget()
        {
            transform.LookAt(_target.transform);
        }
    }
}