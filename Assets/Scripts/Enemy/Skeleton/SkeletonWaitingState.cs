using System.Collections;
using Skullknight.State;
using UnityEngine;

namespace Skullknight.Enemy.Skeleton
{
    public class SkeletonWaitingState : EntityState<Skullknight.Skeleton.ESkeletonState,Skullknight.Skeleton>
    {
        public SkeletonWaitingState(Skullknight.Skeleton _controler) : base(_controler)
        { }

        public override void EnterState()
        {
            controller.SetDamageImmunity(false);
            controller.Animator.Play("Idle");
            coroutines.Add(controller.StartCoroutine(WaitingRoutine(5)));
        }

        private IEnumerator WaitingRoutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            controller.ChangeWaypoint();
            controller.ChangeState(Skullknight.Skeleton.ESkeletonState.Patrol);
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