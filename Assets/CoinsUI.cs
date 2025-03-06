using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Skullknight
{
    public class CoinsUI : MonoBehaviour
    {
        [SerializeField] CanvasGroup coinsGroup;
        [SerializeField] TMP_Text coinsText;
        private const float FADE_IN_DURATION = .5f;
        private const float FADE_OUT_DURATION = 1f;
        private const float SHOW_DURATION = 3f;
        private Tween fadingTween;
        private Coroutine fadeOutCoroutine;
        void Start()
        {
            GameManager.Instance?.OnLevelCoinsChanged.AddListener(OnCoinsChanged);
        }

        private void OnCoinsChanged(int arg0)
        {
            coinsText.text = arg0.ToString();
            if (fadeOutCoroutine != null)
            {
                coinsGroup.alpha = 1f;
                StopCoroutine(fadeOutCoroutine);
                if(fadingTween != null) fadingTween.Kill();
                fadeOutCoroutine = null;
                fadeOutCoroutine = StartCoroutine(FadeOut());
            }
            else
            {
                fadeOutCoroutine = StartCoroutine(FadeInOut());
            }
        }

        private IEnumerator FadeInOut()
        {
            fadingTween = coinsGroup.DOFade(1, FADE_IN_DURATION);
            yield return new WaitForSeconds(FADE_IN_DURATION + SHOW_DURATION);
            fadingTween = coinsGroup.DOFade(0, FADE_OUT_DURATION);
            yield return new WaitForSeconds(FADE_OUT_DURATION);
        }

        private IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(SHOW_DURATION);
            fadingTween = coinsGroup.DOFade(0, FADE_OUT_DURATION);
            yield return new WaitForSeconds(FADE_OUT_DURATION);
        }
    }
}
