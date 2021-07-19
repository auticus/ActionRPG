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

        public IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            yield return FadeIn(1f);
        }

        public IEnumerator FadeOut(float time)
        {
            //cancel any running coroutines
            //run the fadeout coroutine
            if (_currentlyActiveFade != null)
            {
                StopCoroutine(_currentlyActiveFade);
            }
            _currentlyActiveFade = StartCoroutine(FadeOutRoutine(time));
            yield return _currentlyActiveFade;
        }

        public IEnumerator FadeIn(float time)
        {
            if (_currentlyActiveFade != null)
            {
                StopCoroutine(_currentlyActiveFade);
            }
            _currentlyActiveFade = StartCoroutine(FadeInRoutine(time));
            yield return _currentlyActiveFade;
        }

        private IEnumerator FadeOutRoutine(float time)
        {
            while (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        private IEnumerator FadeInRoutine(float time)
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
