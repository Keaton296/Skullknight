using Player.Statemachine;
using TMPro;
using UnityEngine;

namespace Player.UI
{
    public class StateListener : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        void Update()
        {
            text.text = PlayerController.Instance.PlayerState.ToString();
        }
    }
}
