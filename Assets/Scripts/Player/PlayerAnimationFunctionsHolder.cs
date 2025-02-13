using Cinemachine;
using Skullknight.Core;
using Skullknight.Player.Statemachine;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerAnimationFunctionsHolder : MonoBehaviour
    {
        [SerializeField] AudioPlayer feetAudioPlayer;
        [SerializeField] private PlayerController controller;

        public void PlayFootstep()
        {
            feetAudioPlayer.PlayRandom();
        }

        public void Attack()
        {
            controller.SwordAttack();
        }

    }
}
