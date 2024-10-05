using System.Collections;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerRollState : PlayerState
    {
        public PlayerRollState(PlayerController controller) : base(controller)
        {
            this.controller = controller;
        }
    
        public override void OnStateEnd()
        {
         
        }

        public override void OnStateStart()
        {
            controller.Roll();
            controller.animator.SetTrigger("roll");
            controller.StartCoroutine(RollCooldown());
        }

        public override void StateUpdate()
        {
            if (IsRollingAnimationDone())
            {
                float crouch = controller.inputSystem.Default.Crouch.ReadValue<float>();
                if (crouch != 0)
                {
                    controller.PlayerState = controller.SlidingState;
                }
                else
                {
                    controller.PlayerState = controller.IdleState;
                }
            }
        }

        public override void StateFixedUpdate()
        {
        
        }

        private bool IsRollingAnimationDone()
        {
            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Roll") && stateInfo.normalizedTime > 0.99f;
        }
        public IEnumerator RollCooldown()
        {
            yield return new WaitForSeconds(controller.rollingCooldown);
            //controller.canRoll = true;
        }
    }
}
