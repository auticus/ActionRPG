using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Character
{
    public class Experience : MonoBehaviour, ISaveable, IModifierProvider
    {
        [SerializeField] private int experience = 0;
        [SerializeField] private GameObject levelUpFx;

        private int _level = -1;
        private BaseStats _stats;

        private BaseStats Stats
        {
            get
            {
                if (_stats == null)
                {
                    _stats = GetComponent<BaseStats>();
                }

                return _stats;
            }
            set => _stats = value;
        }

        public int GetExperience() => experience;
        public EventHandler OnLevelUp;

        private void Start()
        {
            _level = Stats.GetStartingLevel();
        }

        public void AddExperience(int xp)
        {
            experience += xp;
            if (IsReadyToLevelUp())
            {
                LevelUp();
            }
        }

        public int GetCurrentLevel()
        {
            if (_level < 1)
            {
                _level = Stats.GetStartingLevel();
            }
            return _level;
        }

        /// <summary>
        /// Checks the current level and the current experience and determines if a level up is allowed
        /// </summary>
        /// <returns></returns>
        private bool IsReadyToLevelUp()
        {
            if (_level == 9) return false;  //this is the max level

            var targetXp = Stats.GetTargetXp(_level + 1);
            return experience >= targetXp;
        }

        private void LevelUp()
        {
            _level++;
            OnLevelUp?.Invoke(this, EventArgs.Empty);

            if (levelUpFx != null)
            {
                var fx = Instantiate(levelUpFx, GetFxLocation(), transform.rotation);
                Destroy(fx, 2.0f);
            }
        }

        private Vector3 GetFxLocation()
        {
            //assume capsule collider is roughly same size as player so get half of that value which should be center-mass
            var targetCapsule = GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return transform.position;
            return transform.position + Vector3.up * targetCapsule.height / 1.25f;
        }

        public object CaptureState()
        {
            return new object[] { _level, experience };
        }

        public void RestoreState(object state)
        {
            var data = (object[])state;
            _level = (int)data[0];
            experience = (int)data[1];
        }

        public IEnumerable<int> GetAdditiveModifiers(BaseStats.Stat stat)
        {
            return Enumerable.Empty<int>();
        }

        public IEnumerable<float> GetPercentageModifiers(BaseStats.Stat stat)
        {
            return Enumerable.Empty<float>();
        }
    }
}
