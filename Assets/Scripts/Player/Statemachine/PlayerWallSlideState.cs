using System.Collections;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerWallSlideState : PlayerState
    {
        public PlayerWallSlideState(PlayerController controller) : base(controller)
        {
        
        }

        public override void OnStateEnd()
        {
            // controller.wallSlideParticle.Stop();
        }

        public override void OnStateStart()
        {
            controller.animator.SetTrigger("wallslide");
            // controller.wallSlideParticle.Play();
            controller.rb.velocity = Vector2.up * controller.rb.velocity.magnitude;
        }

        public override void StateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && controller.wallSlideCheckDeathCoroutine == null)
            {
                controller.rb.isKinematic = true;
                controller.rb.velocity = new Vector2(controller.spriteRenderer.flipX ? -controller.maxRunningVelocity : controller.maxRunningVelocity,controller.jumpVelocity);
                controller.rb.isKinematic = false;
                controller.wallSlideCheckDeathCoroutine = controller.StartCoroutine(WallSlideCheckDeath());
                controller.PlayerState = controller.FallingState;
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
