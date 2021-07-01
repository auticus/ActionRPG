using UnityEngine;

namespace Assets.Code.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistantObjectPrefab;

        private static bool hasSpawned = false;

        void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistantObject();
            hasSpawned = true;
        }

        private void SpawnPersistantObject()
        {
            var persistantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}
