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

        private const string WEAPON_NAME = "Weapon";
        
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (weapon != null)
            {
                var wpn = Instantiate(weapon, GetHand(rightHand, leftHand));
                wpn.name = WEAPON_NAME;
            }

            //there can be a bug where if the animator controller is null it will cause the character to use the controller of the previously used weapon
            var overrideControl = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animationOverride != null)
            {
                animator.runtimeAnimatorController = animationOverride;
            }
            else if (overrideControl != null)
            {
                //if already 
                animator.runtimeAnimatorController = overrideControl.runtimeAnimatorController;
            }
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

        private void DestroyOldWeapon(Transform rHand, Transform lHand)
        {
            var old = rHand.Find(WEAPON_NAME);
            if (old == null)
            {
                old = lHand.Find(WEAPON_NAME);
            }

            if (old == null) return;

            old.name = "KILL ME BILLY"; //frames can cause confusion here so lets change the name so nothing else can search for it, then destroy it
            Destroy(old.gameObject);
        }
    }
}