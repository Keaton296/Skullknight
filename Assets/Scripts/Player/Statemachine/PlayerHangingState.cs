using System.Collections;
using Skullknight.Player.Statemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Statemachine
{
    public class PlayerHangingState : PlayerState
    {
        public PlayerHangingState(PlayerController stateManager) : base(stateManager){}

        public override void StateFixedUpdate()
        {
            
        }

        public override void SubscribeEvents()
        {
            controller.playerInput.actions["Hold"].performed += OnHoldPerformed;
        }

        public override void UnsubscribeEvents()
        {
            controller.playerInput.actions["Hold"].performed -= OnHoldPerformed;
        }

        public override void EnterState()
        {
            controller.animator.SetTrigger("hang");
            controller.Hang();
        }
        public override void ExitState()
        {
            
        }

        public override void StateUpdate()
        {
            
        }

        private void OnHoldPerformed(InputAction.CallbackContext context)
        {
            controller.ChangeState(EPlayerState.Climbing);
        }
    }
}
