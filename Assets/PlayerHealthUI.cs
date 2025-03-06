using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Skullknight.Player.Statemachine;
using UnityEngine;
using UnityEngine.UI;
namespace Skullknight
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup coinsGroup;
        [SerializeField] private List<UIPlayerHeart> hearts;
        [SerializeField] private UISpriteContainer container;
        private const float FADE_IN_DURATION = .5f;
        private const float FADE_OUT_DURATION = 1f;
        private const float SHOW_DURATION = 3f;
        private Tween fadingTween;
        private Coroutine fadeOutCoroutine;

        void Start()
        {
            UpdateHearts(PlayerController.Instance.Health);
            PlayerController.Instance.OnHealthChanged.AddListener(UpdateUI);
        }

        void OnDisable()
        {
            PlayerController.Instance.OnHealthChanged.RemoveListener(UpdateUI);
        }

        public void UpdateUI(int newHealth)
        {
            UpdateHearts(newHealth);
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

        private void UpdateHearts(int health)
        {
            int fullhps = health / (container.PlayerHearts.Length - 1);
            health = health - fullhps * (container.PlayerHearts.Length - 1);
            foreach (var heart in hearts)
            {
                if(fullhps > 0)
                {
                    heart.HealthValue = container.PlayerHearts.Length - 1;
                    fullhps--;
                }
                else
                {
                    heart.HealthValue = health;
                    health = 0;
                }
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
