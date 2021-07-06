using RPG.Character;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject weapon = null;
        [SerializeField] private AnimatorOverrideController animationOverride = null;
        
        [SerializeField] private float Range = 2f;
        [SerializeField] private float Damage = 5f;
        [SerializeField] private bool rightHanded = true;
        [SerializeField] private Projectile projectile = null;  //if it has none, its not a projectile
        
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (weapon != null) Instantiate(weapon, GetHand(rightHand, leftHand));
            if (animationOverride != null) animator.runtimeAnimatorController = animationOverride;
        }

        public bool IsRangedWeapon() => projectile != null;

        public void FireProjectile(Transform rHand, Transform lHand, Health target)
        {
            var rangedProjectile = Instantiate(projectile, GetHand(rHand, lHand).position, Quaternion.identity);
            rangedProjectile.SetTarget(target, Damage);
        }

        public float GetRange() => Range;
        public float GetDamage() => Damage;

        private Transform GetHand(Transform rHand, Transform lHand) => rightHanded ? rHand : lHand;
    }
}