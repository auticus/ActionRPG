using UnityEngine;

namespace RPG.Cameras
{
    /// <summary>
    /// Used to rotate the UI canvas so that it faces the camera
    /// </summary>
    public class CameraFacing : MonoBehaviour
    {
        void Update()
        {
            //this is the way the object should be facing
            transform.forward = Camera.main.transform.forward;
        }
    }
}
