using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace Skullknight
{
    public class ScreenShadowUI : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private float _animSpeed;
        private bool isShadow = false;
        public bool IsShadow => isShadow;

        public void ShadowAnimate(ShadowAnimationType animationType)
        {
            if (animationType == ShadowAnimationType.In)
            {
                _animator.SetTrigger("In");
            }
            else if (animationType == ShadowAnimationType.Out)
            {
                _animator.SetTrigger("Out");
            }
        }
        public Action OnShadowLift;
        public Action OnShadowPresent;

        public void OnShadowLifted()
        {
            OnShadowLift?.Invoke();
            isShadow = false;
        }
        public void OnShadowPresented()
        {
            OnShadowPresent?.Invoke();
            isShadow = true;
        }
    }

    public enum ShadowAnimationType
    {
        In,
        Out
    }
}
