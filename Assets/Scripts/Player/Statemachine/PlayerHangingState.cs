using System.Collections;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerHangingState : PlayerState
    {
        public PlayerHangingState(PlayerController controller) : base(controller)
        {
            this.controller = controller;
        }
        public override void OnStateStart()
        {
        
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
}
