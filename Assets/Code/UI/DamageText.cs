using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        private const string TEXT_GAMEOBJECT_NAME = "Text";
        [SerializeField] private float textLifeSeconds = 3.0f;

        public void DestroyText()
        {
            Destroy(gameObject, textLifeSeconds);
        }

        public void SetDamageText(float damage)
        {
            foreach (Transform child in transform)
            {
                if (child.name == TEXT_GAMEOBJECT_NAME)
                {
                    var text = child.GetComponent<Text>();
                    text.text = damage.ToString();
                    return;
                }
            }
        }
    }
}
