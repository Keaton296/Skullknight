using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerOnAirState : MonoBehaviour,IState
{
    [Header("StateController")]
    [SerializeField] PlayerController controller;

    public void OnStateEnd()
    {
        controller.rb.gravityScale = 1f;

    }

    public void OnStateStart()
    {
        controller.rb.gravityScale = 1f;
    }

    public void StateFixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && Mathf.Abs(controller.rb.velocity.x) <= controller.onAirHorizontalSpeedLimit) controller.rb.AddForce(Vector2.right * Input.GetAxisRaw("Horizontal") * controller.onAirStrafingAcceleration,ForceMode2D.Impulse);
        else if (Mathf.Abs(controller.rb.velocity.x) > controller.onAirHorizontalSpeedLimit) controller.rb.velocity = new Vector2(Mathf.Clamp(controller.rb.velocity.x, -controller.onAirHorizontalSpeedLimit, controller.onAirHorizontalSpeedLimit), controller.rb.velocity.y);
        if (controller.groundCollisionChecker.inTrigger && controller.jumpCheckDeathCoroutine == null)  
        {
            controller.PlayerState = controller.playerStandingState;
            controller.PlayLandFX();
           
        }
        else if(controller.wallSlideCheckDeathCoroutine == null && controller.rb.velocity.y > 0)
        {
            RaycastHit2D result = Physics2D.Raycast(transform.position, transform.right, .65f, controller.wallMask);
            if(result.transform != null)
            { // go to wall slide state
                controller.SetFlip(transform.position.x - result.transform.position.x < 0);
                controller.PlayerState = controller.playerWallSlideState;
            }
            result = Physics2D.Raycast(transform.position, -transform.right, .65f, controller.wallMask);
            if (result.transform != null)
            {
                controller.SetFlip(transform.position.x - result.transform.position.x < 0);
                controller.PlayerState = controller.playerWallSlideState;
            }
        }
        if(controller.rb.velocity.y < 0f)
        {
            controller.rb.gravityScale = 1.5f;
        }
        if(controller.rb.velocity.y < -controller.maxFallSpeed)
        {
            controller.rb.AddForce(Vector2.up * -(controller.rb.velocity.y - controller.maxFallSpeed) * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }

    }

    public void StateUpdate()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D result = Physics2D.BoxCast(controller.holdCheckCollider.bounds.center, controller.holdCheckCollider.bounds.size, 0f, controller.rb.velocity, 0f, controller.hangMask);
            if (result.collider != null)
            {
                controller.hangObjectTransform = result.transform;
                controller.hangingObjectCollider = result.transform.parent.GetComponent<Collider2D>();
                controller.PlayerState = controller.playerHangingState;
            }
        }
        if (controller.rb.velocity.y > 1)
        {
            if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) controller.animator.SetTrigger("jump");

        }
        else if ( controller.rb.velocity.y < 0f)
        {
            if (controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Fallbetween") || controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            {

            }
            else 
            { 
                controller.animator.SetTrigger("fallbetween"); 
            }
        }
        /*else
        {
            if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Fall")) controller.animator.SetTrigger("fall");
        }*/
        switch (Input.GetAxisRaw("Horizontal"))
        {
            case 1:
                controller.SetFlip(false);
                break;
            case -1:
                controller.SetFlip(true);
                break;
            default:
                break;
        }

    }
}
