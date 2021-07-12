using System;
using System.Linq;
using UnityEngine;

namespace RPG.Character
{
    public class BaseStats : MonoBehaviour
    {
        public enum Stat
        {
            Experience = 0,
            Health,
            Damage
        }

        [Range(1,9)]
        [SerializeField] private int startingLevel = 1;

        [SerializeField] private CharacterClasses characterClass;
        [SerializeField] private Progression progression = null;

        private Experience _experience;
        private Experience Experience
        {
            get
            {
                if (_experience == null)
                {
                    _experience = GetComponent<Experience>();
                }

                return _experience;
            }
            set => _experience = value;
        }

        /// <summary>
        /// Gets the health of the character based on the level passed
        /// </summary>
        /// <returns></returns>
        public int GetBaseHealth()
        {
            return progression.GetHealth(Experience.GetCurrentLevel());
        }

        /// <summary>
        /// The xp that the character is worth based on their level
        /// </summary>
        /// <returns></returns>
        public int GetBaselineExperience()
        {
            return progression.GetXp(Experience.GetCurrentLevel());
        }

        public int GetTargetXp(int level)
        {
            return progression.GetXp(level);
        }

        public int GetStartingLevel()
        {
            return startingLevel;
        }

        public int GetBaseDamage()
        {
            var baseDamage = progression.GetBaseDamage(Experience.GetCurrentLevel());
            return GetAdditiveModifiers(Stat.Damage, baseDamage);
        }

        private int GetAdditiveModifiers(Stat stat, int baseDamage)
        {
            if (characterClass != CharacterClasses.Player) return baseDamage; //only player characters should use modifiers

            var modifiableComponents = GetComponents<IModifierProvider>();
            baseDamage += modifiableComponents.Select(component =>
                component.GetAdditiveModifiers(Stat.Damage)).Select(modifiers => modifiers.Sum()).Sum();

            var percentageModifier = modifiableComponents.Select(component =>
                component.GetPercentageModifiers(Stat.Damage)).Select(modifiers => modifiers.Sum()).Sum();

            var percentageDamage = (int)Math.Floor(baseDamage * percentageModifier);

            return baseDamage + percentageDamage;
        }
    }
}
