using RPG.Character;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        private Health _healthComponent;
        private Image _healthBar = null;
        private const string FOREGROUND_GAMEOBJECT = "Foreground";

        private void Start()
        {
            _healthComponent = gameObject.transform.parent.gameObject.GetComponent<Health>();
            
            //hierarchy == Health Bar (this object) >> Canvas >> Background >> Foreground
            foreach (Transform canvas in gameObject.transform)
            {
                foreach (Transform background in canvas.transform)
                {
                    foreach (Transform foreground in background.transform)
                    {
                        if (foreground.gameObject.name != FOREGROUND_GAMEOBJECT) continue;
                        _healthBar = foreground.gameObject.GetComponent<Image>();
                    }
                }
            }

            if (_healthBar == null)
            {
                Debug.LogWarning($"Game Object {gameObject.name} HealthBar could not be found");
                return;
            }
        }

        private void Update()
        {
            var healthPercent = _healthComponent.ToPercent();
            _healthBar.transform.localScale = new Vector3(healthPercent, 1, 1);

            if (healthPercent <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
