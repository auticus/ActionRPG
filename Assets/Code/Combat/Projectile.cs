using RPG.Character;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    private Health _target = null;
    private float _damage = 0f;

    private void Update()
    {
        if (_target == null) return;
        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    public void SetTarget(Health target, float damage)
    {
        _target = target;
        _damage = damage;
    }

    void OnTriggerEnter(Collider other)
    {
        var health = other.gameObject.GetComponent<Health>();
        if (health == null || health != _target) return;
        health.Damage(_damage);
        Destroy(gameObject);
    }

    private Vector3 GetAimLocation()
    {
        //assume capsule collider is roughly same size as player so get half of that value which should be center-mass
        var targetCapsule = _target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null) return _target.transform.position;
        return _target.transform.position + Vector3.up * targetCapsule.height / 1.25f;
    }
}
