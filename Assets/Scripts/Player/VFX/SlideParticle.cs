using Player.Statemachine;
using Skullknight.Player.Statemachine;
using UnityEngine;

namespace Player.VFX
{
    [RequireComponent(typeof(ParticleSystem))]
    public class SlideParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;

        void Start()
        {
            if(particleSystem == null) particleSystem = GetComponent<ParticleSystem>();
            PlayerController.Instance.OnStateChange.AddListener(OnSlideHandler);
        }
        void OnSlideHandler(EPlayerState state)
        {
            if (state == EPlayerState.Sliding)
            {
                particleSystem.Play();
            }
            else
            {
                particleSystem.Stop();
            }
        }
    }
}
