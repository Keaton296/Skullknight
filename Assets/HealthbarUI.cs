using System.Collections;
using Skullknight.Core;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace Skullknight
{
    public class HealthbarUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Slider slider_diff;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private MonoBehaviour entity;
        private IDamageable damageable;
        private const float BAR_SHOW_DURATION = 1f;
        private const float BAR_DIFF_SHRINK_DURATION = .25f;
        private const float BAR_FADE_DURATION = 0.5f;
        private Coroutine fadeCoroutine;
        private Tween fadeTween;
        private Tween diffTween;

        void Awake()
        {
            if(slider) slider = GetComponent<Slider>();
            if(canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        }
        void Start()
        {
            
        }

        void OnEnable()
        {
            damageable = entity as IDamageable;
            if (damageable != null)
            {
                damageable.OnHealthChanged.AddListener(UpdateSlider);
                slider.maxValue = damageable.MaxHealth;
                slider.value = damageable.Health;
                slider_diff.maxValue = damageable.MaxHealth;
                slider_diff.value = damageable.Health;
            }
        }

        private void OnDisable()
        {
            if (damageable != null)
            {
                damageable.OnHealthChanged.RemoveListener(UpdateSlider);
            }
        }

        private void UpdateSlider(int newHp)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }
            if(fadeTween != null)
            {
                fadeTween.Kill();
                fadeTween = null;
            }

            if (diffTween != null)
            {
                diffTween.Kill();
                diffTween = null;
            }
            slider.value = newHp;
            fadeCoroutine = StartCoroutine(FadeCoroutine());
        }

        private IEnumerator FadeCoroutine()
        {
            canvasGroup.alpha = 1;
            diffTween = slider_diff.DOValue(damageable.Health, BAR_DIFF_SHRINK_DURATION, false);
            yield return new WaitForSecondsRealtime(BAR_SHOW_DURATION);
            fadeTween = canvasGroup.DOFade(0, BAR_FADE_DURATION);
            yield return new WaitForSeconds(BAR_FADE_DURATION);
            diffTween = null;
            fadeTween = null;
            fadeCoroutine = null;
        }
    }
}
