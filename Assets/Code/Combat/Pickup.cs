using System.Collections;
using RPG.Controllers;
using RPG.UI;
using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Weapon weapon = null;
        [SerializeField] private float respawnTime = 5.0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;
            PickItUp(other.GetComponent<Fighter>());
        }

        private void PickItUp(Fighter player)
        {
            player.EquipWeapon(weapon);

            //Destroy(gameObject);  //we are no longer destroying we want to respawn it
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

        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickItUp(controller.GetComponent<Fighter>());
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.ItemPickup;
        }
    }
}
