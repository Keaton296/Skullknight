using System.Collections;
using Player.Statemachine;
using Skullknight.Player.Statemachine;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class StaminaUI : MonoBehaviour
    {
        [SerializeField] Image staminaImage;
        private Coroutine staminaCoroutine;
        private float FADEAWAY_SECONDS = 1f;
        void Awake()
        {
            if(staminaImage == null) staminaImage = GetComponent<Image>();
            staminaImage.CrossFadeAlpha(0f,0,true);
        }

        void Start()
        {
            PlayerController.Instance.OnRoll.AddListener(StaminaLoadAnimation);
        }

        void OnDestroy()
        {
            PlayerController.Instance.OnRoll.RemoveListener(StaminaLoadAnimation);
        }
        public void StaminaLoadAnimation()
        {
            if(staminaCoroutine != null) StopCoroutine(staminaCoroutine);
            staminaCoroutine = StartCoroutine(StaminaLoad());
        }

        public IEnumerator StaminaLoad()
        {
            staminaImage.CrossFadeAlpha(1f,0,true);
            while (PlayerController.Instance.Stamina != PlayerController.Instance.MaxStamina)
            {
                staminaImage.fillAmount = PlayerController.Instance.Stamina / PlayerController.Instance.MaxStamina;
                yield return null;
            }
            staminaImage.CrossFadeAlpha(0f,FADEAWAY_SECONDS,false);
        }

    }
}
