using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingState : MonoBehaviour, IState
{
    [Header("PlayerController")]
    [SerializeField] PlayerController controller;

    public void OnStateEnd()
    {
        
    }

    public void OnStateStart()
    {
        controller.ActiveBoxCollider2D = controller.standingCollider;
    }

    public void StateFixedUpdate()
    {
        if (controller.groundCollisionChecker.inTrigger)
        {
            //if(Input.GetAxisRaw("Horizontal") != 0) controller.rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * controller.runningVelocity,controller.rb.velocity.y);
            //else controller.rb.velocity = Vector2.zero;
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                controller.HorizontalMove(PlayerHorizontalMoveType.Run);
            }
        }
        else
        {
            controller.PlayerState = controller.playerOnAirState;
        }
    }

    public void StateUpdate()
    {
        if (Input.GetKeyDown(controller.rollKey) && controller.canRoll && Input.GetAxisRaw("Horizontal") != 0)
        {
            controller.StartCoroutine(controller.Roll());
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            controller.PlayerState = controller.playerAttackingState;
            return;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (Mathf.Abs(controller.rb.velocity.x) > controller.groundSlideRequiredHorizontalSpeed) controller.PlayerState = controller.playerSlidingState;
            else controller.PlayerState = controller.playerCrouchingState;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && controller.jumpCheckDeathCoroutine == null && controller.groundCollisionChecker.inTrigger)
        {
            controller.Jump();
            controller.PlayerState = controller.playerOnAirState;
            return;
        }
        if(Input.GetAxisRaw("Horizontal") == 0)
        {
            if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) controller.animator.SetTrigger("idle");
            controller.ActiveBoxCollider2D.sharedMaterial = controller.stoppingPhysicMaterial;
        }
    }
}
