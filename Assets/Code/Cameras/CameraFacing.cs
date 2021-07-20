using UnityEngine;

namespace RPG.Cameras
{
    /// <summary>
    /// Used to rotate the UI canvas so that it faces the camera
    /// </summary>
    public class CameraFacing : MonoBehaviour
    {
        void LateUpdate()
        {
            //this is the way the object should be facing
            //LateUpdate because we want this to happen AFTER the model rotates
            transform.forward = Camera.main.transform.forward;
        }
    }
}
