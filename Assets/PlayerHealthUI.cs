using System.Collections;
using System.Collections.Generic;
using Skullknight.Player.Statemachine;
using UnityEngine;
using UnityEngine.UI;
namespace Skullknight
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private List<UIPlayerHeart> hearts;
        [SerializeField] private UISpriteContainer container;

        void Start()
        {
            UpdateUI(PlayerController.Instance.Health);
            PlayerController.Instance.OnHealthChanged.AddListener(UpdateUI);
        }

        void OnDisable()
        {
            PlayerController.Instance.OnHealthChanged.RemoveListener(UpdateUI);
        }

        public void UpdateUI(int newHealth)
        {
            int fullhps = newHealth / (container.PlayerHearts.Length - 1);
            newHealth = newHealth - fullhps * (container.PlayerHearts.Length - 1);
            foreach (var heart in hearts)
            {
                if(fullhps > 0)
                {
                    heart.HealthValue = container.PlayerHearts.Length - 1;
                    fullhps--;
                }
                else
                {
                    heart.HealthValue = newHealth;
                    newHealth = 0;
                }
            }
        }
    }
}
