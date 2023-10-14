using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : MonoBehaviour, IState
{
    [Header("PlayerController")]
    [SerializeField] PlayerController controller;
    public void OnStateEnd()
    {
        controller.wallSlideParticle.Stop();
    }

    public void OnStateStart()
    {
        controller.animator.SetTrigger("wallslide");
        controller.wallSlideParticle.Play();
        controller.rb.velocity = Vector2.up * controller.rb.velocity.magnitude;
    }

    public void StateFixedUpdate()
    {
        if (controller.groundCollisionChecker.inTrigger && controller.jumpCheckDeathCoroutine == null)
        {
            controller.PlayerState = controller.playerStandingState;
        }
    }

    public void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.wallSlideCheckDeathCoroutine == null)
        {
            controller.rb.isKinematic = true;
            controller.rb.velocity = new Vector2(controller.spriteRenderer.flipX ? -controller.maxRunningVelocity : controller.maxRunningVelocity,controller.jumpVelocity);
            controller.rb.isKinematic = false;
            controller.wallSlideCheckDeathCoroutine = controller.StartCoroutine(controller.WallSlideCheckDeath());
            controller.PlayerState = controller.playerOnAirState;
        }
        
    }
}
