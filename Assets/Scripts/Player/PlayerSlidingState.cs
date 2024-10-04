using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerSlidingState : PlayerState
{
    public PlayerSlidingState(PlayerController controller) : base(controller)
    {
        this.controller = controller;
    }

    public override void OnStateEnd()
    {
        controller.crouchCollider.sharedMaterial = controller.normalPhysicMaterial;
    }

    public override void OnStateStart()
    {
        controller.ActiveBoxCollider2D = controller.crouchCollider;
        controller.crouchCollider.sharedMaterial = controller.slidingPhysicMaterial;
        controller.animator.SetTrigger("slide");
        controller.Groundslide();
        controller.OnSlide?.Invoke();
    }

    public override void StateUpdate()
    {
        float crouch = controller.inputSystem.Default.Crouch.ReadValue<float>();
        if (crouch != 1)
        {
            controller.rb.velocity = Vector2.zero; 
            if (controller.standUpCollisionChecker.IsColliding)
            {
                controller.PlayerState = controller.CrouchState;
            }
            else
            {
                controller.PlayerState = controller.IdleState;
            }
        }
    }

    public override void StateFixedUpdate()
    {
        float crouch = controller.inputSystem.Default.Crouch.ReadValue<float>();
        if (!controller.groundCollisionChecker.IsColliding)
        {
            controller.PlayerState = controller.FallingState;
        }
        else if (Mathf.Abs(controller.rb.velocity.x) < controller.SlideStoppingSpeed)
        {
            if(crouch != 0) controller.PlayerState = controller.CrouchState;
            else if (!controller.standUpCollisionChecker.IsColliding)
            {
                controller.PlayerState = controller.IdleState;
            }
            else
            {
                controller.PlayerState = controller.CrouchState;
            }
        }
    }
}
