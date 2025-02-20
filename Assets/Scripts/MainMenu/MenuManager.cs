using System.Collections;
using System.Collections.Generic;
using Skullknight.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


namespace Skullknight
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Animator shadowAnimator; 
        [SerializeField] private BossMainScreenAnimator bossAnimator;

        void Awake()
        {
            Time.timeScale = 1;
        }
        public void OnPlayPressed()
        {
            bossAnimator.Terminate();
            StartCoroutine(PlayButtonPressed());
        }

        public void OnQuitPressed()
        {
            bossAnimator.Terminate();
            Application.Quit();
        }

        IEnumerator PlayButtonPressed()
        {
            EventSystem.current.enabled = false;
            shadowAnimator.Play("Circle_In");
            yield return null;
            yield return new WaitUntil(() => shadowAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
}
