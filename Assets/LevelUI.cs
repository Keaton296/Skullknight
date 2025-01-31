using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Skullknight
{
    public class LevelUI : MonoBehaviour
    {
        public static LevelUI Instance;
        public Animator CutsceneUIAnimator => cutsceneUIAnimator;
        [SerializeField] private Animator cutsceneUIAnimator;

        void Awake()
        {
            if (Instance == null) Instance = this;
        }
    }
}
