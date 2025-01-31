using Skullknight.Core;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerAnimationFunctionsHolder : MonoBehaviour
    {
        [SerializeField] AudioPlayer feetAudioPlayer;

        public void PlayFootstep()
        {
            feetAudioPlayer.PlayRandom();
        }

    }
}
