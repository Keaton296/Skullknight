using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skullknight.Enemy.Demon_Boss
{
    public class DemonBossLaserAttack : DemonBossState
    {
        public DemonBossLaserAttack(DemonBossController _controller) : base(_controller)
        { }

        private IEnumerator AttackingRoutine()
        {
            controller.canTurn = true;
            controller.animator.Play("LaserAttack");
            yield return null;
            yield return new WaitUntil(() => controller.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f);
            controller.ChangeState(EDemonBossState.Idle);
        }
        public override void EnterState()
        {
            coroutines.Add(controller.StartCoroutine(AttackingRoutine()));
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
    }
}