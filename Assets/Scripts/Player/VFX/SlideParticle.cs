using Player.Statemachine;
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
        void OnSlideHandler(PlayerState state)
        {
            if (state is PlayerSlidingState)
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
