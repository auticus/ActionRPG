using UnityEngine;

namespace RPG.Core
{
    public class PostEffectCleanup : MonoBehaviour
    {
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}