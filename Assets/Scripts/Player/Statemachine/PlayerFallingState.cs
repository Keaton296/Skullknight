using Skullknight.Player.Statemachine;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerFallingState : PlayerState
    {

        public PlayerFallingState(PlayerController _controller) : base(_controller){}
        public override void EnterState()
        {
            controller.rb.gravityScale = controller.FallingGravityScale;
            controller.Animator.Play("Fallbetween");
        } 
        public override void ExitState()
        {
            controller.rb.gravityScale = 1f;
        }

        public override void StateUpdate()
        {
            
        }

        public override void StateFixedUpdate()
        {
            float horizontal = controller.playerInput.actions["Horizontal"].ReadValue<float>();
            if (controller.rb.velocity.y < -controller.MaxFallingVelocity)
            {
                controller.rb.velocity = new Vector3(controller.rb.velocity.x, -controller.MaxFallingVelocity, 0f);
            }
            if (controller.groundCollisionChecker.IsColliding && controller.CanJump)
            {
                controller.landingAudioPlayer.PlayRandom();
                controller.ChangeState(EPlayerState.Idle);
            }
            else if (horizontal != 0)
            {
                controller.Airstrafe(horizontal);
            }
        }

        public override void SubscribeEvents()
        {
            controller.playerInput.actions["Hold"].performed += controller.OnHoldPerformed;
        }

        public override void UnsubscribeEvents()
        {
            controller.playerInput.actions["Hold"].performed -= controller.OnHoldPerformed;
        }
    }
}
