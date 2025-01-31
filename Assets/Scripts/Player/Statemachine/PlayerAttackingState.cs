using Skullknight.Player.Statemachine;
using Skullknight.State;
using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerAttackingState : PlayerState
    { //The code softlocks in this state
        public PlayerAttackingState(PlayerController controller) : base(controller){}

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

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            controller.currentActionCoroutine = null;
        }
    }
}
