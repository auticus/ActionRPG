using RPG.Character;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Resources
{
    class ExperienceDisplay : MonoBehaviour
    {
        private Experience _experience;
        private Text _label;

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            _label = GetComponent<Text>();
        }

        private void Update()
        {
            _label.text = $"Level: {_experience.GetCurrentLevel()}  XP: {_experience.GetExperience()}";
        }
    }
}