using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Skullknight
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance => m_instance;
        public static PlayerInput PlayerInput => m_playerInput;
        private static InputManager m_instance;
        [SerializeField] private static PlayerInput m_playerInput;
        
        void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }

            if (m_playerInput == null)
            {
                m_playerInput = GetComponent<PlayerInput>();
            }

            GameManager.Instance.OnStateChange.AddListener(OnGameManagerStateChanged);
        }

        private void OnGameManagerStateChanged(GameManager.EGameManagerState switchedState)
        {
            switch (switchedState)
            {
                case GameManager.EGameManagerState.Playing:
                    ChangeCurrentActionMap("Default");
                    break;
                case GameManager.EGameManagerState.EscapeMenu:
                    ChangeCurrentActionMap("UI");
                    break;
                case GameManager.EGameManagerState.Cutscene:
                    ChangeCurrentActionMap("UI");
                    break;
                case GameManager.EGameManagerState.Gameover:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(switchedState), switchedState, null);
            }
        }
        public void ChangeCurrentActionMap(string mapName)
        {
            m_playerInput.SwitchCurrentActionMap(mapName);
        }
    }
}
