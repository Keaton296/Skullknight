using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Skullknight
{
    public class InputReader : MonoBehaviour
    {
        public static InputReader Singleton;
        private PlayerInputActions _playerInputActions;
        public PlayerInputActions InputActions => _playerInputActions;
        
        private void Awake()
        {
            if(Singleton != null)
            {
                Debug.Log("Singleton already exists!");
                Destroy(gameObject);
            }
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            _playerInputActions = new PlayerInputActions();
        }
        
    }
}
