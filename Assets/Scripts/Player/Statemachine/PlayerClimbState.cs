using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerClimbState : PlayerState
    {
        // Start is called before the first frame update
        public PlayerClimbState(PlayerController controller) : base(controller)
        {
            this.controller = controller;
        }

        public override void OnStateStart()
        {
            controller.animator.SetTrigger("climb");
        }

        public override void OnStateEnd()
        {
            controller.Unhang();
        }

        public override void StateUpdate()
        {
            if (IsClimbingAnimationDone())
            {
                controller.PlayerState = controller.IdleState;
            }
        }

        private bool IsClimbingAnimationDone()
        {
            AnimatorStateInfo state = controller.animator.GetCurrentAnimatorStateInfo(0);
            return state.IsName("Climb") && state.normalizedTime >= 0.95f;
        }
    }
}
