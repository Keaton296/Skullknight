using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFunctionsHolder : MonoBehaviour
{
    [SerializeField] AudioPlayer feetAudioPlayer;

    public void PlayFootstep()
    {
        feetAudioPlayer.Play();
    }

}
