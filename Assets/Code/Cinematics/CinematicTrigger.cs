using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _hasPlayed;
        private void OnTriggerEnter(Collider other)
        {
            if (!_hasPlayed && other.gameObject.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                _hasPlayed = true;
            }
        }
    }
}
