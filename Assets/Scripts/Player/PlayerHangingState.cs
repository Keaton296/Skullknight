using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHangingState : MonoBehaviour,IState
{
    [Header("PlayerController")]
    [SerializeField] PlayerController controller;
    public void OnStateEnd()
    {
        
    }

    public void OnStateStart()
    {
        controller.rb.isKinematic = true;
        Physics2D.IgnoreCollision(controller.standingCollider, controller.hangingObjectCollider, true);
        Physics2D.IgnoreCollision(controller.crouchCollider, controller.hangingObjectCollider, true);
        controller.playerSpriteOffset = controller.spriteRenderer.transform.localPosition;
        controller.SetFlip(controller.hangObjectTransform.position.x - transform.position.x < 0);

        controller.animator.SetTrigger("hang");


        controller.hangObjectHangPointTransform = controller.hangObjectTransform.GetChild(0);
        controller.hangObjectClimbPointTransform = controller.hangObjectTransform.GetChild(1);
        transform.parent = controller.hangObjectTransform;

        transform.localPosition = controller.hangObjectHangPointTransform.localPosition;
        controller.rb.velocity = Vector2.zero;
    }

    public void StateFixedUpdate()
    {
        
    }

    public void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && controller.currentActionCoroutine == null) {
            controller.currentActionCoroutine = controller.StartCoroutine(controller.Climb());
        }
    }
}
