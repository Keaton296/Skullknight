using Skullknight.Player.Statemachine;
using Skullknight.State;
using UnityEngine.InputSystem;

namespace Player.Statemachine
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerController controller) : base(controller){}

        public override void EnterState()
        {
            controller.ActiveBoxCollider2D = controller.standingCollider;
            controller.ActiveBoxCollider2D.sharedMaterial = controller.stoppingPhysicMaterial;
        }

        public override void ExitState()
        {
            
        }
    

        public override void StateUpdate()
        {
            if(!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) controller.animator.SetTrigger("idle");
        
            controller.RegenerateStamina();
        
            if (controller.playerInput.actions["Crouch"].IsPressed()) controller.ChangeState(EPlayerState.Crouching);
            else if (controller.playerInput.actions["Horizontal"].IsPressed()) controller.ChangeState(EPlayerState.Running);
        }
        public override void StateFixedUpdate()
        {
            if (!controller.groundCollisionChecker.IsColliding)
            {
                //fall
            }
        }

        public override void SubscribeEvents()
        {
            controller.playerInput.actions["Jump"].performed += controller.OnJumpPerformed;
            controller.playerInput.actions["Attack"].performed += OnAttackPerformed;
        }
        
        public override void UnsubscribeEvents()
        {
            controller.playerInput.actions["Jump"].performed -= controller.OnJumpPerformed;
            controller.playerInput.actions["Attack"].performed -= OnAttackPerformed;
        }

        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            controller.ChangeState(EPlayerState.AttackOne);
        }
    }
}
