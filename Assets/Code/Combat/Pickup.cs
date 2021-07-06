using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Pick me up!!");
            if (other.gameObject.tag != "Player") return;
            var player = other.GetComponent<Fighter>();
            player.EquipWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
