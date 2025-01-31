using Skullknight.Player.Statemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Statemachine
{
    public class PlayerRunningState : PlayerState
    {
        public PlayerRunningState(PlayerController stateManager) : base(stateManager)
        {
            this.controller = stateManager;
        }

        public override void EnterState()
        {
            controller.ActiveBoxCollider2D.sharedMaterial = controller.normalPhysicMaterial;
        
            controller.Run(controller.playerInput.actions["Horizontal"].ReadValue<float>());
        }
        public override void ExitState()
        {
            
        }

        private void OnHorizontalCanceled(InputAction.CallbackContext context)
        {
            controller.ChangeState(EPlayerState.Idle);
        }

        public override void StateUpdate()
        {
            float velocityProgress = Mathf.Abs(controller.rb.velocity.x) / controller.maxRunningVelocity;
        
            controller.RegenerateStamina();
            
            if (controller.playerInput.actions["Crouch"].IsPressed())
            {
                if(Mathf.Abs(controller.rb.velocity.x) > controller.SlidingSpeed) controller.ChangeState(EPlayerState.Sliding);
                else
                {
                    controller.ChangeState(EPlayerState.Crouching);
                }
            }
            else if (controller.playerInput.actions["Roll"].IsPressed())
            {
                controller.ChangeState(EPlayerState.Roll);
            }
            else if (!controller.playerInput.actions["Horizontal"].IsPressed())
            {
                controller.ChangeState(EPlayerState.Idle);
            }
            else
            {
                if(!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) controller.animator.SetTrigger("run");
                controller.animator.SetFloat("runAnimSpeed", velocityProgress * controller.maxRunningAnimSpeed);
            }
        
        }

        public override void StateFixedUpdate()
        {
            //if grounded and run, crouch key held and has enough velocity, slide
            //if grounded and run key held, run
            //if only grounded, idle
            //if not grounded, fall
            float horizontal = controller.playerInput.actions["Horizontal"].ReadValue<float>();
            if (controller.groundCollisionChecker.IsColliding) 
            {
                if (controller.playerInput.actions["Horizontal"].IsPressed())
                {
                    controller.Run(horizontal);
                }
            }
            else
            {
                controller.ChangeState(EPlayerState.Falling);
            }
        }

        public override void SubscribeEvents()
        {
            controller.playerInput.actions["Horizontal"].canceled += OnHorizontalCanceled;
            controller.playerInput.actions["Jump"].performed += controller.OnJumpPerformed;
        }

        public override void UnsubscribeEvents()
        {
            controller.playerInput.actions["Horizontal"].canceled -= OnHorizontalCanceled;   
            controller.playerInput.actions["Jump"].performed -= controller.OnJumpPerformed;
        }
    }
}