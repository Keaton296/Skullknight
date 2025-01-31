using System.Collections;
using System.Collections.Generic;
using Skullknight.Player.Statemachine;
using Skullknight.State;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerClimbingState : PlayerState
    {
        public PlayerClimbingState(PlayerController controller) : base(controller){}

        public override void StateFixedUpdate()
        {
            
        }

        public override void SubscribeEvents()
        {
            
        }

        public override void UnsubscribeEvents()
        {
            
        }

        public override void EnterState()
        {
            controller.animator.SetTrigger("climb");
        }

        public override void ExitState()
        {
            controller.animator.SetTrigger("idle");
            controller.Unhang();
        }

        public override void StateUpdate()
        {
            if (IsClimbingAnimationDone())
            {
                controller.ChangeState(EPlayerState.Idle);
            }
        }

        private bool IsClimbingAnimationDone()
        {
            AnimatorStateInfo state = controller.animator.GetCurrentAnimatorStateInfo(0);
            return state.IsName("Climb") && state.normalizedTime >= 0.95f;
        }
        private IEnumerator DelayedKinematicDisable()
        {
            yield return new WaitForFixedUpdate();
            controller.rb.isKinematic = false;
        }
    }
}
