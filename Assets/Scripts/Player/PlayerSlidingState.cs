using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlidingState : MonoBehaviour,IState
{
    [SerializeField] PlayerController controller;
    public void OnStateEnd()
    {
        controller.crouchCollider.sharedMaterial = controller.normalPhysicMaterial;
        controller.slideParticle.Stop();
    }

    public void OnStateStart()
    {
        controller.ActiveBoxCollider2D = controller.crouchCollider;
        controller.crouchCollider.sharedMaterial = controller.slidingPhysicMaterial;
        controller.slideParticle.Play();
        if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Slide")) controller.animator.SetTrigger("slide");
    }

    public void StateFixedUpdate()
    {
        if (Input.GetKey(KeyCode.S) && Mathf.Abs(controller.rb.velocity.x) > controller.groundSlideStoppingSpeed)
        {
            return;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            controller.PlayerState = controller.playerCrouchingState;
        }
        else if(!controller.standUpCollisionChecker.inTrigger)
        {
            controller.PlayerState = controller.playerStandingState;
        }
    }

    public void StateUpdate()
    {

    }
    
}
