using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchingState : MonoBehaviour,IState
{
    [Header("PlayerController")]
    [SerializeField] PlayerController controller;
    public void OnStateEnd()
    {
        controller.ActiveBoxCollider2D = controller.standingCollider;
    }

    public void OnStateStart()
    {
        controller.ActiveBoxCollider2D = controller.crouchCollider;
    }

    public void StateFixedUpdate()
    {
        if (controller.groundCollisionChecker.inTrigger)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                controller.HorizontalMove(PlayerHorizontalMoveType.Crouch);
            }
        }
        else
        {
            controller.PlayerState = controller.playerOnAirState;
        }
        
    }

    public void StateUpdate()
    {
        
        if(Input.GetAxisRaw("Horizontal") == 0)
        {
            if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch")) controller.animator.SetTrigger("crouch");
        }
        if (Input.GetKey(KeyCode.S))
        {
            return;
        }
        else
        {
            if (controller.standUpCollisionChecker.inTrigger)
            {
                return;
            }
            else
            {
                controller.PlayerState = controller.playerStandingState;
            }
        }
    }
}
