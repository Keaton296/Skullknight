using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skullknight
{
    public class TestComponent : MonoBehaviour
    {
        private PlayerInputActions _inputActions;
        void Start()
        {
            //InputModuleTest();
        }

        private void InputModuleTest()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.UI.Enable();
            
        }
    }
}
