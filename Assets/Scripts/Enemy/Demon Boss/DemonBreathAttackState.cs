using System.Collections;
using Skullknight.State;
using UnityEngine;

namespace Skullknight.Enemy.Demon_Boss
{
    public class DemonBreathAttackState : EntityState<EDemonBossState,DemonBossController>
    {
        public DemonBreathAttackState(DemonBossController _controller) : base(_controller)
        { }

        public override void EnterState()
        {
            controller.canTurn = true;
            coroutines.Add(controller.StartCoroutine(AttackRoutine()));
        }

        public override void ExitState()
        {
            
        }

        public override void StateUpdate()
        {
            if (controller.canTurn)
            {
                controller.LookPlayer();
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

        public IEnumerator AttackRoutine()
        {
            controller.Animator.Play("Breath");
            yield return null;
            yield return new WaitUntil(() => controller.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f);
            controller.Animator.Play("Idle");
            controller.MoveToIdlePosition();
            yield return new WaitForSeconds(1f);
            controller.ChangeState(EDemonBossState.Idle);
        }
    }
}