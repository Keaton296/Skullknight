using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHangingState : PlayerState
{
    public PlayerHangingState(PlayerController controller) : base(controller)
    {
        this.controller = controller;
    }
    public override void OnStateStart()
    {
        controller.rb.isKinematic = true;
        Physics2D.IgnoreCollision(controller.standingCollider, controller.hungObjectCollider, true);
        Physics2D.IgnoreCollision(controller.crouchCollider, controller.hungObjectCollider, true);
        //controller.playerSpriteOffset = controller.spriteRenderer.transform.localPosition;
        controller.SetFlip(controller.hangObjectTransform.position.x - controller.transform.position.x < 0);

        controller.animator.SetTrigger("hang");


        controller.hangObjectHangPointTransform = controller.hangObjectTransform.GetChild(0);
        controller.hangObjectClimbPointTransform = controller.hangObjectTransform.GetChild(1);
        controller.transform.parent = controller.hangObjectTransform;

        controller.transform.localPosition = controller.hangObjectHangPointTransform.localPosition;
        controller.rb.velocity = Vector2.zero;
    }
    public override void OnStateEnd()
    {
        
    }
    public IEnumerator Climb()
    {
        /*controller.animator.SetTrigger("climb");

        yield return new WaitUntil(() => climbAnimEnded);

        climbAnimEnded = false;
        transform.localPosition = hangObjectClimbPointTransform.localPosition;
        spriteTransform.localPosition = playerSpriteOffset;
        transform.parent = null;
        rb.velocity = Vector2.zero;
        rb.isKinematic = false;
        currentActionCoroutine = null;
        animator.SetTrigger("idle");
        Physics2D.IgnoreCollision(standingCollider, hangingObjectCollider, false);
        Physics2D.IgnoreCollision(crouchCollider, hangingObjectCollider, false);
        controller.PlayerState = controller.StandingState;*/
        yield return null;
    }
    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && controller.currentActionCoroutine == null) {
            controller.currentActionCoroutine = controller.StartCoroutine(Climb());
        }
    }
}
