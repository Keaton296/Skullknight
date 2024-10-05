using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Statemachine
{
    public class PlayerCrouchingState : PlayerState
    {

        public PlayerCrouchingState(PlayerController controller) : base(controller)
        {
            this.controller = controller;
        }

        public override void OnStateEnd()
        {
            controller.inputSystem.Default.Crouch.canceled -= OnCrouchCanceled;
        }

        public override void OnStateStart()
        {
            controller.ActiveBoxCollider2D = controller.crouchCollider;
            controller.animator.SetTrigger("crouch");
        
            controller.inputSystem.Default.Crouch.canceled += OnCrouchCanceled;
        }

        private void OnCrouchCanceled(InputAction.CallbackContext context)
        {
            if (!controller.standUpCollisionChecker.IsColliding)
            {
                controller.PlayerState = controller.IdleState;   
            }
        }


        public override void StateUpdate()
        {
            controller.RegenerateStamina();
        
            float horizontal = controller.inputSystem.Default.Horizontal.ReadValue<float>();
        
            if (horizontal != 0)
            {
                if(!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Crouchwalk")) controller.animator.SetTrigger("crouchwalk");
                float crouchWalkProgress = Mathf.Abs(controller.rb.velocity.x) / controller.maxCrouchingVelocity;
                controller.animator.SetFloat("crouchWalkSpeed",crouchWalkProgress);
            }
            else
            {
                if(!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch")) controller.animator.SetTrigger("crouch");
            }
        }

        public override void StateFixedUpdate()
        {
            float horizontal = controller.inputSystem.Default.Horizontal.ReadValue<float>();
            float crouch = controller.inputSystem.Default.Crouch.ReadValue<float>();
            if (controller.groundCollisionChecker.IsColliding)
            {
                if(horizontal != 0) controller.Crouchwalk(horizontal);
                if (crouch == 0 && !controller.standUpCollisionChecker.IsColliding) controller.PlayerState = controller.IdleState;
            }
            else
            {
                //to falling
                throw new NotImplementedException();
            }
        }
    }
}
