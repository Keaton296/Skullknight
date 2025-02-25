using System.Collections;
using UnityEngine;
using xKhano.StateMachines.Entities;


namespace Skullknight.Enemy.Demon_Boss
{
    public class DemonBossFireballAttackState : EntityState<EDemonBossState,DemonBossController>
    {
        public DemonBossFireballAttackState(DemonBossController _controller) : base(_controller)
        { }
        public override void EnterState()
        {
            controller.canTurn = true;
            coroutines.Add(controller.StartCoroutine(FireballAttack()));
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

        private IEnumerator FireballAttack()
        {
            controller.Animator.Play("Fireball");
            yield return null;
            yield return new WaitUntil(() =>controller.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f);
            controller.Animator.Play("Idle");
            controller.MoveToIdlePosition();
            yield return new WaitForSeconds(1f);
            controller.ChangeState(EDemonBossState.Idle);
        }
    }
}