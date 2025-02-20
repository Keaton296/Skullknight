using System.Collections;
using Skullknight.State;
using UnityEngine;

namespace Skullknight.Enemy.Skeleton
{
    public class SkeletonAttackingState : EntityState<Skullknight.Skeleton.ESkeletonState,Skullknight.Skeleton>
    {
        public SkeletonAttackingState(Skullknight.Skeleton _controler) : base(_controler)
        { }

        private IEnumerator Attack()
        {
            controller.Animator.Play("AttackOne");
            yield return null;
            yield return new WaitUntil(()=> controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("AttackOne") && 
                                            controller.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f);
            controller.ChangeState(Skullknight.Skeleton.ESkeletonState.Chasing);
        }
        public override void EnterState()
        {
            controller.SetDamageImmunity(false);
            controller.LookPlayer();
            coroutines.Add(controller.StartCoroutine(Attack()));
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