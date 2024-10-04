using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFallingState : PlayerState
{

    public PlayerFallingState(PlayerController controller) : base(controller)
    {
        this.controller = controller;
    }
    public override void OnStateStart()
    {
        controller.rb.gravityScale = controller.FallingGravityScale;
        controller.animator.SetTrigger("fallbetween");
    } 
    public override void OnStateEnd()
    {
        controller.rb.gravityScale = 1f;
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateFixedUpdate()
    {
        float horizontal = controller.inputSystem.Default.Horizontal.ReadValue<float>();
        if (controller.rb.velocity.y < -controller.MaxFallingVelocity)
        {
            controller.rb.velocity = new Vector3(controller.rb.velocity.x, -controller.MaxFallingVelocity, 0f);
        }
        if (controller.groundCollisionChecker.IsColliding && controller.CanJump)
        {
            controller.PlayerState = controller.IdleState;
        }
        else if (horizontal != 0)
        {
            controller.Airstrafe(horizontal);
        }
    }
    
}
