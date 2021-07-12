using RPG.Character;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;
        private Text _healthLabel;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
            _healthLabel = GetComponent<Text>();
        }

        private void Update()
        {
            _healthLabel.text = $"Health: {_health.ToPercent()}";
        }
    }
}