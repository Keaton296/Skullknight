using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerState
{
    public PlayerRunningState(PlayerController controller) : base(controller)
    {
        this.controller = controller;
    }

    public override void OnStateStart()
    {
        controller.ActiveBoxCollider2D.sharedMaterial = controller.normalPhysicMaterial;
        
        controller.Run(controller.inputSystem.Default.Horizontal.ReadValue<float>());
        
        controller.inputSystem.Default.Horizontal.canceled += OnHorizontalCanceled;
        controller.inputSystem.Default.Jump.performed += controller.OnJumpPerformed;
    }
    public override void OnStateEnd()
    {
        controller.inputSystem.Default.Horizontal.canceled -= OnHorizontalCanceled;   
        controller.inputSystem.Default.Jump.performed -= controller.OnJumpPerformed;
    }

    private void OnHorizontalCanceled(InputAction.CallbackContext context)
    {
        controller.PlayerState = controller.IdleState;
    }

    public override void StateUpdate()
    {
        float velocityProgress = Mathf.Abs(controller.rb.velocity.x) / controller.maxRunningVelocity;
        
        controller.RegenerateStamina();
        
        float crouch = controller.inputSystem.Default.Crouch.ReadValue<float>();
        float horizontal = controller.inputSystem.Default.Horizontal.ReadValue<float>();
        float roll = controller.inputSystem.Default.Roll.ReadValue<float>();
        if (crouch != 0)
        {
            if(Mathf.Abs(controller.rb.velocity.x) > controller.SlidingSpeed) controller.PlayerState = controller.SlidingState;
            else
            {
                controller.PlayerState = controller.CrouchState;
            }
        }
        else if (roll != 0)
        {
            controller.PlayerState = controller.RollState;
        }
        else if (horizontal == 0)
        {
            controller.PlayerState = controller.IdleState;
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
        float horizontal = controller.inputSystem.Default.Horizontal.ReadValue<float>();
        if (controller.groundCollisionChecker.IsColliding) 
        {
             if (horizontal != 0)
             {
                 controller.Run(horizontal);
             }
        }
        else
        {
            controller.PlayerState = controller.FallingState;
        }
    }
}