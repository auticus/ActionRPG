using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,
            B,
            C,
            D
        }

        [SerializeField] private int sceneToLoad;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeOutTime = 0.5f;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("The scene to load is not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject); //dont destroy the portal this is attached to (otherwise the scene will destroy it when its gone)
            //this only works when the scene is at the root of the scene

            var fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            //save the current level
            var wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            //SceneManager.LoadScene(sceneToLoad);
            yield return SceneManager.LoadSceneAsync(sceneToLoad); //run this up until its loaded, when finished loading we will continue

            //load current level
            wrapper.Load();

            var otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject); //now lets destroy the portal now that we dont need it anymore
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            //you can have a problem with nav mesh agent jacking things up - so tell the navmesh agent where to warp to to prevent this
            var player =GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            //player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }

            return null;
        }
    }
}