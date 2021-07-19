using UnityEngine;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab = null;

        public void Spawn(float damage)
        {
            var instance = Instantiate<DamageText>(damageTextPrefab, parent: transform);
            instance.SetDamageText(damage);
            instance.DestroyText();
        }
    }
}
