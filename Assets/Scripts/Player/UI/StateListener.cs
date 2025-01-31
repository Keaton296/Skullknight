using System;
using Player.Statemachine;
using Skullknight.Player.Statemachine;
using TMPro;
using UnityEngine;

namespace Player.UI
{
    public class StateListener : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private PlayerController player;

        void OnEnable()
        {
            player.OnStateChange.AddListener(OnPlayerStateChanged);
        }
        void OnDisable()
        {
            player.OnStateChange.RemoveListener(OnPlayerStateChanged);
        }

        private void OnPlayerStateChanged(EPlayerState state)
        {
            text.text = state.ToString();
        }
    }
}
