using System.Collections;
using Player.Statemachine;
using UnityEngine;

namespace Skullknight.Player.Statemachine
{
    public class PlayerCrouchingAttackState : PlayerState
    {
        public PlayerCrouchingAttackState(PlayerController playerController) : base(playerController)
        {
        }

        private IEnumerator CrouchAttack()
        {
            controller.animator.Play("CrouchAttack");
            yield return null;
            yield return new WaitUntil(()=>controller.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f);
            controller.ChangeState(EPlayerState.Crouching);
        }
        public override void EnterState()
        {
            coroutines.Add(controller.StartCoroutine(CrouchAttack()));
        }

        public override void ExitState()
        {
            
        }

        public override void StateUpdate()
        {
            
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
    }
}