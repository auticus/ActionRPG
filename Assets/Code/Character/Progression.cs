using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private int[] health = new int[10];
        [SerializeField] private int[] experience = new int[10];
        [SerializeField] private int[] baseDamage = new int[10];

        public int GetHealth(int index)
        {
            return health[index];
        }

        /// <summary>
        /// Returns the xp needed to go up to the level given
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetXp(int index)
        {
            return experience[index];
        }

        public int GetBaseDamage(int index)
        {
            return baseDamage[index];
        }
    }
}
