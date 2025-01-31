using System.Collections;
using System.Collections.Generic;
using Skullknight.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Skullknight
{
    public class EscapeMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject firstSelectedButton;
        [SerializeField] private GameObject escapeMenu;

        void Awake()
        {
            GameManager.Instance.OnStateChange.AddListener(OnGameManagerStateChange);
        }
        private void OnGameManagerStateChange(GameManager.EGameManagerState switchedState)
        {
            switch (switchedState)
            {
                case GameManager.EGameManagerState.Playing:
                    escapeMenu.SetActive(false);
                    break;
                case GameManager.EGameManagerState.EscapeMenu:
                    escapeMenu.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(firstSelectedButton);
                    break;
            }
        }
    }
}
