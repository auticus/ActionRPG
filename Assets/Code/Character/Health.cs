using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Saving;
using RPG.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Character
{
    public class Health : MonoBehaviour, ISaveable, IModifierProvider
    {
        private int _health = -1; //setting this to -1 so that start has something to evaluate and not stomp on loaded state
        private Animator _animator;
        private CapsuleCollider _collider;
        private float _maxHealth;
        private BaseStats _baseStats;
        private Experience _experience;
        private DamageTextSpawner _dmgTextSpawner = null;
        
        private const string DAMAGE_TEXT_SPAWNER_NAME = "Damage Text Spawner";
        private const string HEALTH_BAR_NAME = "Health Bar";
        

        public bool IsDead { get; private set; }

        void Start()
        {
            Initialize();

            foreach (Transform child in transform)
            {
                if (_dmgTextSpawner == null && child.name == DAMAGE_TEXT_SPAWNER_NAME)
                {
                    _dmgTextSpawner = child.GetComponent<DamageTextSpawner>();
                    break;
                }
            }
        }

        public void Damage(int dmg)
        {
            if (IsDead) return;  //if he's already dead... dont need to keep damaging him

            _health -= dmg;

            if (_health < 0) _health = 0;
            DisplayDamage(dmg);

            if (_health == 0) Die();
        }

        public object CaptureState()
        {
            return _health;
        }

        public void RestoreState(object state)
        {
            //beware of saving hell!  the start here may go after the load, etc...
            //the saving system appears to run well before any of the objects initialize so things like the animator may not be created yet
            Initialize();
            _health = (int)state;

            if (_health == 0)
                Die();
        }

        /// <summary>
        /// Displays the health in a current/max health format
        /// </summary>
        /// <returns></returns>
        public string ToDisplayLong()
        {
            return $"{_health} / {_maxHealth}";
        }

        public float ToPercent() => _health / _maxHealth;

        public IEnumerable<int> GetAdditiveModifiers(BaseStats.Stat stat)
        {
            return Enumerable.Empty<int>();
        }

        public IEnumerable<float> GetPercentageModifiers(BaseStats.Stat stat)
        {
            return Enumerable.Empty<float>();
        }

        private void Initialize()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                _collider = GetComponent<CapsuleCollider>();
                _maxHealth = GetComponent<BaseStats>().GetBaseHealth();
                _baseStats = GetComponent<BaseStats>();
                _experience = GetComponent<Experience>();
                _experience.OnLevelUp += OnLevelUp;
            }

            if (_health < 0) //its the -1 default which is not valid so get its base health
            {
                _health = GetComponent<BaseStats>().GetBaseHealth();
            }
        }

        private void OnLevelUp(object sender, EventArgs e)
        {
            SetHealth(_baseStats.GetBaseHealth());
        }
        private void SetHealth(int health)
        {
            _health = health;
            _maxHealth = health;
        }

        private void Die()
        {
            _animator.SetTrigger("Die");
            IsDead = true;
            _collider.enabled = false;
        }

        private void DisplayDamage(int dmg)
        {
            //all characters have a Damage Text Spawner gameobject childed
            //this has a DamageTextSpawner script

            _dmgTextSpawner.Spawn((float)dmg);
        }
    }
}
