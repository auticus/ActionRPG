using System;
using RPG.Audio;
using RPG.Character;
using RPG.Controllers;
using RPG.UI;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class Target : MonoBehaviour, IRaycastable
    {
        public enum TargetStatus
        {
            Alive = 0,
            Dead
        }

        public TargetStatus Status => _health.IsDead ? TargetStatus.Dead : TargetStatus.Alive;
        public bool IsPlayer => _isPlayer;

        public EventHandler onDeath;
        public Guid TargetID = Guid.NewGuid();

        private Health _health;
        private BaseStats _stats;
        private bool _isPlayer;
        private bool _diedEventFired = false;

        private SoundFx _soundFx;

        void Start()
        {
            _health = GetComponent<Health>();
            _stats = GetComponent<BaseStats>();
            _soundFx = GetComponent<SoundFx>();
            _isPlayer = this.CompareTag("Player");
        }

        public void Hit(int dmg, HitSource hitSource)
        {
            var hitSound = GetSoundSource(hitSource);
            _soundFx.Play(hitSound);

            _health.Damage(dmg);
            if (_health.IsDead && !_diedEventFired)
            {
                _diedEventFired = true;
                onDeath?.Invoke(this, EventArgs.Empty);
            }
        }

        public Health GetHealth() => _health;
        public int GetXp() => _stats.GetBaselineExperience();

        public bool HandleRaycast(PlayerController controller)
        {
            if (!controller.GetComponent<Fighter>().IsValidTarget(this))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                controller.GetComponent<Fighter>().Attack(this);
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        private SoundFx.SoundSource GetSoundSource(HitSource hitSource)
        {
            switch (hitSource)
            {
                case HitSource.Sword:
                    return SoundFx.SoundSource.MeleeHit;
                case HitSource.Bow:
                    return SoundFx.SoundSource.RangedHit;
                case HitSource.Fireball:
                    return SoundFx.SoundSource.FireballHit;
            }

            return SoundFx.SoundSource.MeleeHit;
        }
    }
}
