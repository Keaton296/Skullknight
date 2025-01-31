using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skullknight
{
    public class UITransitionManager : MonoBehaviour
    {
        public static UITransitionManager Instance;
        [SerializeField] public Animator screenFocuserAnimator;
        [SerializeField] public Animator blackoutAnimator;
        [SerializeField] public Animator levelTextAnimator;

        private void Awake()
        {
            Instance = this;
        }
        //playing an animation from the controller.
        //waiting that animation to finish
    }
}
