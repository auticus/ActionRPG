using RPG.Character;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private bool isHoming = false;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 10.0f;

        [SerializeField]
        private GameObject[] destroyOnHit = null; //the gameobjects we chain through to destroy when we hit

        [SerializeField] private float lifeAfterImpact = 2.0f; //how long the object will live after we collide

        private Target _target = null;
        private int _damage = 0;

        private void Start()
        {
            if (_target == null) return;
            transform.LookAt(
                GetAimLocation()); //when this spawns have it point at the target, but after that it flies in a straight direction
        }

        private void Update()
        {
            if (_target == null) return;
            if (isHoming && !_target.GetHealth().IsDead)
                transform.LookAt(GetAimLocation()); //only home in if the health is not dead
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        public void SetTarget(Target target, int damage)
        {
            _target = target;
            _damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        void OnTriggerEnter(Collider other)
        {
            var target = other.gameObject.GetComponent<Target>();
            if (target == null || target != _target) return;

            if (target.GetHealth().IsDead) return; //if he's dead just keep on flying

            target.Hit(_damage);

            speed = 0; //we hit, there is no more speed

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            //Destroy the destroy on hit array
            foreach (var obj in destroyOnHit)
            {
                Destroy(obj);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        private Vector3 GetAimLocation()
        {
            //assume capsule collider is roughly same size as player so get half of that value which should be center-mass
            var targetCapsule = _target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return _target.transform.position;
            return _target.transform.position + Vector3.up * targetCapsule.height / 1.25f;
        }
    }
}