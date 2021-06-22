using System.Diagnostics;
using RPG.Character;
using RPG.Combat;
using UnityEngine;

namespace RPG.Controllers
{
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Health))]
    public class EntityController : MonoBehaviour
    {
        protected Movement movement;
        protected Fighter fighter;
        protected Health health;
        private bool _handledPlayerDeath = false;

        protected void Initialize()
        {
            movement = GetComponent<Movement>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        protected bool HandleDeath()
        {
            //returns TRUE if the character is dead, otherwise false
            if (health.IsDead && _handledPlayerDeath) return true;
            if (health.IsDead && !_handledPlayerDeath)
            {
                movement.Cancel();
                fighter.Cancel();
                _handledPlayerDeath = true;
                return true;
            }

            return false;
        }
    }
}
