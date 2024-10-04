using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController controller) : base(controller)
    {
        this.controller = controller;
    }
    public override void OnStateStart()
    {
        controller.animator.SetTrigger("jump");
        controller.Jump();
        controller.inputSystem.Default.Hold.performed += OnHoldPerformed;
    }

    public void OnHoldPerformed(InputAction.CallbackContext context)
    {
        Collider2D deneme = controller.GetHangPoint();
        if (deneme != null)
        {
            //controller.
        }
    }

    public override void OnStateEnd()
    {
        
    }

    public override void StateUpdate()
    {
        float jump = controller.inputSystem.Default.Jump.ReadValue<float>();
        if (jump != 1)
        {
            controller.JumpCut();
            //controller.PlayerState = controller.FallingState;
        }
    }

    public override void StateFixedUpdate()
    {
        float horizontal = controller.inputSystem.Default.Horizontal.ReadValue<float>();
        if (controller.CanJump && controller.groundCollisionChecker.IsColliding)
        {
            controller.PlayerState = controller.IdleState;
        }
        else if (controller.rb.velocity.y < 0)
        {
            controller.PlayerState = controller.FallingState;
        }
        else if (horizontal != 0)
        {
            controller.Airstrafe(horizontal);
        }
    }
}
