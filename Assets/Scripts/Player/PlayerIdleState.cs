using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController controller) : base(controller)
    {
        this.controller = controller;
    }

    public override void OnStateEnd()
    {
        controller.inputSystem.Default.Jump.performed -= controller.OnJumpPerformed;
    }

    public override void OnStateStart()
    {
        controller.ActiveBoxCollider2D = controller.standingCollider;
        controller.ActiveBoxCollider2D.sharedMaterial = controller.stoppingPhysicMaterial;
        
        controller.inputSystem.Default.Jump.performed += controller.OnJumpPerformed;
    }
    

    public override void StateUpdate()
    {
        if(!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) controller.animator.SetTrigger("idle");
        
        controller.RegenerateStamina();
        
        float horizontal = controller.inputSystem.Default.Horizontal.ReadValue<float>();
        float crouch = controller.inputSystem.Default.Crouch.ReadValue<float>();
        float jump = controller.inputSystem.Default.Jump.ReadValue<float>();
        
        if (crouch == 1) controller.PlayerState = controller.CrouchState;
        else if (horizontal != 0) controller.PlayerState = controller.RunningState;
    }
    public override void StateFixedUpdate()
    {
        if (!controller.groundCollisionChecker.IsColliding)
        {
            //fall
        }
    }
}
