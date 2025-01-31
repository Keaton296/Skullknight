using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;
namespace Skullknight
{
    public class IntroScreen : MonoBehaviour
    {
        [SerializeField] private GameObject skipText;
        [SerializeField] private GameObject firstSelectedButton;
        [SerializeField] private PlayableDirector director;
        [SerializeField] private Image shadowImage;
        void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }
        public void OnBegin()
        {
            EventSystem.current.SetSelectedGameObject(skipText);
        }

        public void OnEnd()
        {
            gameObject.SetActive(false);
            shadowImage.color = new Color(0, 0, 0, 0);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }

        public void OnSkip()
        {
            director.Stop();
            OnEnd();
        }
    }
}
