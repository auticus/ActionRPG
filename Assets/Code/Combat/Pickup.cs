using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon = null;
        [SerializeField] private float respawnTime = 5.0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;
            var player = other.GetComponent<Fighter>();
            player.EquipWeapon(weapon);
            //Destroy(gameObject);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            //cannot disable items or else coroutines stop too!
            HidePickup();
            yield return new WaitForSeconds(seconds);
            ShowPickup();
        }

        private void HidePickup()
        {
            SetActiveState(false);
        }

        private void ShowPickup()
        {
            SetActiveState(true);
        }

        private void SetActiveState(bool active)
        {
            //disable the collider
            var box = GetComponent<BoxCollider>();
            box.enabled = active;

            //disable child game objects
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(active);
            }
        }
    }
}
