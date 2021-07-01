using RPG.Character;
using RPG.Combat;
using UnityEditor.UIElements;
using UnityEngine;

namespace RPG.Controllers
{
    public class AIController : EntityController
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointWaitTime = 2f;

        private Target _playerTarget;
        private Vector3 _currentDestination;
        private float _currentWaitTime = Mathf.Infinity;
        
        private float DistanceFromPlayer => Vector3.Distance(transform.position, _playerTarget.transform.position);
        private bool IsPlayerInRange => DistanceFromPlayer < chaseDistance;

        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        
        void Start()
        {
            Initialize();
            _playerTarget = GameObject.FindWithTag("Player").GetComponent<Target>();

            _currentDestination = patrolPath != null ? patrolPath.GetCurrentWaypoint() : transform.position;
        }

        void Update()
        {
            if (HandleDeath())
            {
                return;
            }
            if (IsPlayerInRange && fighter.IsValidTarget(_playerTarget))
            {
                _timeSinceLastSawPlayer = 0;
                PursueAndAttackPlayer();
            }
            else if(_timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                DefaultBehavior();
            }

            _timeSinceLastSawPlayer += Time.deltaTime;
        }

        //Called by Unity but only if item is selected
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
            //Gizmos.DrawSphere(transform.position, chaseDistance); this sphere will be fully colored in
        }
        
        private void PursueAndAttackPlayer()
        {
            movement.ChangeMovementState(Movement.MovementState.Pursuit);
            transform.LookAt(_playerTarget.transform);
            fighter.Attack(_playerTarget);
        }

        private void DefaultBehavior()
        {
            var distance = Vector3.Distance(transform.position, _currentDestination);
            movement.ChangeMovementState(Movement.MovementState.Default);
            if (patrolPath == null) return;

            //are we near the stop radius of our next destination?  if so lets go to our next waypoint
            if ( distance <= 1)
            {
                _currentDestination = patrolPath.GetNextWaypoint();
                _currentWaitTime = 0f;
                return;
            }

            _currentWaitTime += Time.deltaTime;
            if (_currentWaitTime < waypointWaitTime) return;
            movement.StartMoveAction(_currentDestination);
        }

        private void SuspicionBehavior()
        {
            GetComponent<Scheduler>().CancelCurrentAction();
        }
    }
}
