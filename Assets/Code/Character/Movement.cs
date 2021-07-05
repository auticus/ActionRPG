using RPG.Combat;
using RPG.Controllers;
using RPG.Interfaces;
using RPG.Saving;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Character
{
    public class Movement : MonoBehaviour, IAction, ISaveable
    {
        public enum MovementState
        {
            Stopped,
            Default,
            Pursuit
        }

        private NavMeshAgent _navMeshAgent;
        private Ray _targetRay;
        private Scheduler _scheduler;
        private Animator _animator;
        [SerializeField] private float defaultSpeed;
        [SerializeField] private float pursuitSpeed;
        
        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _scheduler = GetComponent<Scheduler>();
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 point)
        {
            //_scheduler.StartAction(this);
            _navMeshAgent.destination = point;
            _navMeshAgent.isStopped = false;
        }

        public void ChangeMovementState(MovementState state)
        {
            _navMeshAgent.speed = state switch
            {
                MovementState.Stopped => 0,
                MovementState.Default => defaultSpeed,
                MovementState.Pursuit => pursuitSpeed,
                _ => _navMeshAgent.speed
            };
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SetNavMeshAgent();
            _navMeshAgent.enabled = false; //stops navmeshagent from meddling with what we are doing
            transform.position = ((SerializableVector3) state).ToVector();
            _navMeshAgent.enabled = true;
        }

        private void SetNavMeshAgent()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void UpdateAnimator()
        {
            var velocity = _navMeshAgent.velocity;
            var localVelocity =
                transform.InverseTransformDirection(velocity); //transforms a world and turning into local space
            //when creating velocity we are storing here we are grabbing the global velocity and it may be X = 273 and Y = 7
            //thats not useful for the animator.  Animator just wants to know local velocity to say to animator you are moving forward at 3 units etc
            //global values are not as useful from local point of view

            var speed = localVelocity.z; //need to know the zed speed in a forward direction
            _animator.SetFloat("ForwardSpeed", speed); //ForwardSpeed is set on the animator Blend Tree
        }
    }
}