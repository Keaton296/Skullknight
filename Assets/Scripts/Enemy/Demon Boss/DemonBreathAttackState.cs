using System.Collections;
using UnityEngine;

namespace Skullknight.Enemy.Demon_Boss
{
    public class DemonBreathAttackState : DemonBossState
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
            controller.animator.Play("Breath");
            yield return null;
            yield return new WaitUntil(() => controller.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f);
            controller.animator.Play("Idle");
            controller.MoveToIdlePosition();
            yield return new WaitForSeconds(1f);
            controller.ChangeState(EDemonBossState.Idle);
        }
    }
}