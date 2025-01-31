using Skullknight.Player.Statemachine;
using UnityEngine.InputSystem;

namespace Player.Statemachine
{
    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(PlayerController stateManager) : base(stateManager){}
        public override void EnterState()
        {
            controller.animator.SetTrigger("jump");
            controller.Jump();
        }
        public override void ExitState()
        {
            
        }

        public override void StateUpdate()
        {
            float jump = controller.playerInput.actions["Jump"].ReadValue<float>();
            if (jump != 1)
            {
                controller.JumpCut();
                //controller.PlayerState = controller.FallingState;
            }
        }

        public override void StateFixedUpdate()
        {
            float horizontal = controller.playerInput.actions["Horizontal"].ReadValue<float>();
            if (controller.CanJump && controller.groundCollisionChecker.IsColliding)
            {
                controller.ChangeState(EPlayerState.Idle);
            }
            else if (controller.rb.velocity.y < 0)
            {
                controller.ChangeState(EPlayerState.Falling);
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
