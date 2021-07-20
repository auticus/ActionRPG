using System;
using UnityEngine;

namespace RPG.Audio
{
    public class SoundFx : MonoBehaviour
    {
        public enum SoundSource
        {
            TakeDamage,
            Die,
            MeleeHit,
            RangedHit,
            FireballHit,
            RangedShot,
            FireballShot
        }

        public void Play(SoundSource sound)
        {
            Play(sound, delay: 0);
        }

        /// <summary>
        /// Will attempt to find the game object associated with the sound passed and play it
        /// </summary>
        /// <param name="sound"></param>
        public void Play(SoundSource sound, ulong delay)
        {
            var name = GetGameObjectName(sound);
            var soundObject = GetSoundGameObject(name);
            if (soundObject == null) return;

            soundObject.GetComponent<AudioSource>().PlayDelayed(delay);
        }

        private GameObject GetSoundGameObject(string name)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name.Equals(name, StringComparison.OrdinalIgnoreCase)) return child.gameObject;
            }

            return null;
        }

        private string GetGameObjectName(SoundSource sound)
        {
            switch (sound)
            {
                case SoundSource.TakeDamage:
                    return "Damage Sound";
                case SoundSource.Die:
                    return "Death Sound";
                case SoundSource.FireballHit:
                    return "Fireball Hit Sound";
                case SoundSource.MeleeHit:
                    return "Melee Hit Sound";
                case SoundSource.RangedHit:
                    return "Ranged Hit Sound";
                case SoundSource.RangedShot:
                    return "Bow Shot Sound";
                case SoundSource.FireballShot:
                    return "Fireball Shot Sound";
                default:
                    return "";
            }
        }
    }
}
