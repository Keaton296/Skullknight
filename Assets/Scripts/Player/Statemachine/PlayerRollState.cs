using System.Collections;
using Skullknight.Player.Statemachine;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerRollState : PlayerState
    {
        public PlayerRollState(PlayerController stateManager) : base(stateManager)
        {
            this.controller = stateManager;
        }
        public override void EnterState()
        {
            controller.SetDamageImmunity(true);
            controller.Roll();
            controller.Animator.Play("Roll");
        }
    
        public override void ExitState()
        {
            controller.SetDamageImmunity(false);
        }


        public override void StateUpdate()
        {
            if (IsRollingAnimationDone())
            {
                float crouch = controller.playerInput.actions["Crouch"].ReadValue<float>();
                if (crouch != 0)
                {
                    controller.ChangeState(EPlayerState.Sliding);
                }
                else
                {
                    controller.ChangeState(EPlayerState.Idle);
                }
            }
        }

        public override void StateFixedUpdate()
        {
        
        }

        public override void SubscribeEvents()
        {
            
        }

        public override void UnsubscribeEvents()
        {
            
        }

        private bool IsRollingAnimationDone()
        {
            AnimatorStateInfo stateInfo = controller.Animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Roll") && stateInfo.normalizedTime > 0.99f;
        }
    }
}
