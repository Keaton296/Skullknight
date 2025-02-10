using System.Collections;
using System.Collections.Generic;
using Skullknight.State;
using UnityEngine;

namespace Skullknight.Enemy.Demon_Boss
{
    public class DemonBossFireballAttackState : DemonBossState
    {
        public DemonBossFireballAttackState(DemonBossController _controller) : base(_controller)
        { }
        public override void EnterState()
        {
            coroutines.Add(controller.StartCoroutine(FireballAttack()));
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

        private IEnumerator FireballAttack()
        {
            controller.animator.Play("Fireball");
            yield return null;
            yield return new WaitUntil(() =>!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Fireball"));
            controller.MoveToIdlePosition();
            yield return new WaitForSeconds(1f);
            controller.ChangeState(EDemonBossState.Idle);
        }
    }
}