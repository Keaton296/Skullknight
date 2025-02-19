using System.Collections;
using Skullknight.Player.Statemachine;
using Skullknight.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Statemachine
{
    public class PlayerAttackingState : PlayerState
    {
        private string AtkAnimationName;
        private float atkDuration = .5f;
        private float comboDuration = .5f;
        private bool canMove = false;
        private bool canCombo = false;
        private EPlayerState? nextAttack;
        private Coroutine attackRoutine;
        public PlayerAttackingState(PlayerController controller, string attackAnimation, float _atkDuration,float _comboDuration, EPlayerState? _nextAttack) : base(controller)
        {
            AtkAnimationName = attackAnimation;
            atkDuration = _atkDuration;
            comboDuration = _comboDuration;
            nextAttack = _nextAttack;
        }

        public override void StateUpdate()
        {
            //hangi animasyon olucak bunu bilmek lazim
            //sonraki animasyonu bilecek bir sistem gerek
            if(canMove)
            {
                if (controller.playerInput.actions["Crouch"].IsPressed())
                    controller.ChangeState(EPlayerState.Crouching);
                else if (controller.playerInput.actions["Horizontal"].IsPressed())
                    controller.ChangeState(EPlayerState.Running);
            }
        }
        public override void StateFixedUpdate()
        {
            
        }
        public override void SubscribeEvents()
        {
            controller.playerInput.actions["Attack"].performed += OnAttackButtonPressed;
            controller.playerInput.actions["Jump"].performed += OnJumpButtonPressed;
        }
        public override void UnsubscribeEvents()
        {
            controller.playerInput.actions["Attack"].performed -= OnAttackButtonPressed;
            controller.playerInput.actions["Jump"].performed -= OnJumpButtonPressed;
        }

        private void OnAttackButtonPressed(InputAction.CallbackContext obj)
        {
            if (canCombo && nextAttack.HasValue)
            {
                controller.ChangeState(nextAttack.Value);
            }
        }

        private void OnJumpButtonPressed(InputAction.CallbackContext obj)
        {
            if (canMove)
            {
                controller.OnJumpPerformed(obj);
            }
        }

        public override void EnterState()
        {
            canMove = false;
            canCombo = false;
            attackRoutine = controller.StartCoroutine(AttackCoroutine());
        }

        IEnumerator AttackCoroutine()
        {
            controller.Animator.Play(AtkAnimationName);
            yield return new WaitForSeconds(atkDuration);
            canMove = true;
            canCombo = true;
            yield return new WaitForSeconds(comboDuration);
            controller.ChangeState(EPlayerState.Idle);
        }
        public override void ExitState()
        {
            controller.StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }
}
