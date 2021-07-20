using RPG.Controllers;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControl : MonoBehaviour
    {
        private GameObject _player;
        void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            _player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector director)
        {
            _player.GetComponent<PlayerControlState>().SetEnabled(false);
        }

        void EnableControl(PlayableDirector director)
        {
            _player.GetComponent<PlayerControlState>().SetEnabled(true);
        }
    }
}
