using System.Collections;
using Skullknight.Player.Statemachine;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerWallSlideState : PlayerState
    {
        public PlayerWallSlideState(PlayerController stateManager) : base(stateManager)
        {
        
        }

        public override void ExitState()
        {
            // controller.wallSlideParticle.Stop();
        }

        public override void StateFixedUpdate()
        {
            
        }

        public override void SubscribeEvents()
        {
            
        }

        public override void UnsubscribeEvents()
        {
            
        }

        public override void EnterState()
        {
            controller.Animator.Play("WallSlide");
            // controller.wallSlideParticle.Play();
            controller.rb.velocity = Vector2.up * controller.rb.velocity.magnitude;
        }

        public override void StateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && controller.wallSlideCheckDeathCoroutine == null)
            {
                controller.rb.isKinematic = true;
                controller.rb.velocity = new Vector2(controller.SpriteRenderer.flipX ? -controller.maxRunningVelocity : controller.maxRunningVelocity,controller.jumpVelocity);
                controller.rb.isKinematic = false;
                controller.wallSlideCheckDeathCoroutine = controller.StartCoroutine(WallSlideCheckDeath());
                controller.ChangeState(EPlayerState.Falling);
            }
        
        }

        public IEnumerator WallSlideCheckDeath()
        {
            //yield return new WaitForSeconds(wallSlideCheckDeathSeconds);
            //wallSlideCheckDeathCoroutine = null;
            yield return null;
        }
    }
}
