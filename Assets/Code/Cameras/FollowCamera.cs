using UnityEngine;

namespace RPG.Cameras
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private GameObject FollowTarget; //the player or whatever the camera is going to follow

        // Update is called once per frame
        void LateUpdate()
        {
            //placed the LateUpdate code so that the animations can run in update BEFORE we move the camera to prevent jittering
            transform.position = FollowTarget.transform.position;
        }
    }
}
