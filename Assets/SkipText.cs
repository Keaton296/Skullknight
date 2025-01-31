using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
namespace Skullknight
{
    public class SkipText : MonoBehaviour
    {
        [SerializeField] private int pressCount = 0;
        [SerializeField] private TMP_Text proText;
        [SerializeField] private Image key0;
        [SerializeField] private Image key1;
        public UnityEvent OnSkip;

        void OnEnable()
        {
            var inputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            inputModule.submit.action.performed += OnSkipButtonClickedHandler;
            inputModule.leftClick.action.performed += OnSkipButtonClickedHandler;
        }

        void OnDisable()
        {
            var inputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            inputModule.submit.action.performed -= OnSkipButtonClickedHandler;
            inputModule.leftClick.action.performed -= OnSkipButtonClickedHandler;
        }
        public void OnPress()
        {
            pressCount++;
            if (pressCount == 1)
            {
                Color opaque = new Color(255, 255, 255, 255);
                proText.color = opaque;
                key0.color = opaque;
                key1.color = opaque;

            }
            else
            {
                Color transparent = new Color(255, 255, 255, 0);
                OnSkip?.Invoke();
                proText.color = transparent;
                key0.color = transparent;
                key1.color = transparent;
                pressCount = 0;
            }
        }

        private void OnSkipButtonClickedHandler(InputAction.CallbackContext context)
        {
            OnPress();
        }
    }
}
