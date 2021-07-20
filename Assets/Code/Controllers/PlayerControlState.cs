using UnityEngine;

namespace RPG.Controllers
{
    public class PlayerControlState : MonoBehaviour
    {
        public void SetEnabled(bool state)
        {
            if (state) EnableControl();
            else DisableControl();
        }

        private void DisableControl()
        {
            gameObject.GetComponent<Scheduler>().CancelCurrentAction();
            gameObject.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl()
        {
            gameObject.GetComponent<PlayerController>().enabled = true;
        }
    }
}
