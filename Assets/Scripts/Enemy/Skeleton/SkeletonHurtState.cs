using System.Collections;
using Skullknight.Entity;
using Skullknight.State;
using UnityEngine;

namespace Skullknight.Enemy.Skeleton
{
    public class SkeletonHurtState : EntityState<Skullknight.Skeleton.ESkeletonState,Skullknight.Skeleton>
    {
        public SkeletonHurtState(Skullknight.Skeleton _controler) : base(_controler)
        {
        }

        private IEnumerator Hurt()
        {
            yield return new WaitUntil(() => controller.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f && controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"));
            if(controller.chasing) controller.ChangeState(Skullknight.Skeleton.ESkeletonState.Chasing);
            else controller.ChangeState(Skullknight.Skeleton.ESkeletonState.Patrol);
        }

        public override void EnterState()
        {
            controller.Animator.Play("Hurt");
            controller.SetDamageImmunity(false);
            coroutines.Add(controller.StartCoroutine(Hurt()));
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