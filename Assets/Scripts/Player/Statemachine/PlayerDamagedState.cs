using System.Collections;
using Player.Statemachine;
using UnityEngine;

namespace Skullknight.Player.Statemachine
{
    public class PlayerDamagedState : PlayerState
    {
        public PlayerDamagedState(PlayerController playerController) : base(playerController)
        {
        }

        public override void EnterState()
        {
            coroutines.Add(controller.StartCoroutine(GroundWait(1.5f)));
            //fall to ground
            //wait a bit
            //maybe health reducing animation
            //get up
            //give player immunity time
        }

        private IEnumerator GroundWait(float duration)
        {
            controller.animator.Play("Fallground");
            controller.bumpImpulse.GenerateImpulse();
            yield return new WaitForSeconds(duration);
            controller.animator.Play("Getup");
            yield return null;
            yield return new WaitUntil(()=> controller.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f);
            controller.ChangeState(EPlayerState.Idle);
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