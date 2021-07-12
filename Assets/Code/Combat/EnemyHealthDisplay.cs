using RPG.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Health _health;
        private Text _healthLabel;

        private void Awake()
        {
            _healthLabel = GameObject.Find("lblEnemyHealth").GetComponent<Text>();
        }

        private void Update()
        {
            if (_health == null)
            {
                _healthLabel.text = string.Empty;
                return;
            }

            _healthLabel.text = $"Enemy: {_health.ToPercent()}";
        }

        public void SetTarget(Health target)
        {
            _health = target;
        }
    }
}
