using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        Coroutine _currentlyActiveFade = null;

        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {
            return FadeTo(1, time);
        }

        public Coroutine FadeIn(float time)
        {
            return FadeTo(0, time);
        }

        private Coroutine FadeTo(float target, float time)
        {
            if (_currentlyActiveFade != null)
            {
                StopCoroutine(_currentlyActiveFade);
            }
            _currentlyActiveFade = StartCoroutine(Fade(target,time));
            return _currentlyActiveFade;
        }

        private IEnumerator Fade(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
