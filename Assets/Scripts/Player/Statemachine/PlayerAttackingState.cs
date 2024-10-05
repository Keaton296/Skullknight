using UnityEngine;

namespace Player.Statemachine
{
    public class PlayerAttackingState : PlayerState
    {

        public PlayerAttackingState(PlayerController controller) : base(controller)
        {
            this.controller = controller;
        }

        public override void OnStateEnd()
        {
            controller.currentActionCoroutine = null;
        }

        public override void OnStateStart()
        {
           
        }
    
        public override void StateUpdate()
        {
            
        
        } 
    }
}
