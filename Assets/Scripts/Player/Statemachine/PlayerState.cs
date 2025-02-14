using System.Collections.Generic;
using Skullknight.Player.Statemachine;
using Skullknight.State;
using UnityEngine;

namespace Player.Statemachine
{
    public abstract class PlayerState : BaseState<EPlayerState>
    {
        protected PlayerController controller;
        public List<Coroutine> coroutines;
        public PlayerState(PlayerController playerController)
        {
            controller = playerController;
            coroutines = new List<Coroutine>();
        }
        public void KillCoroutines()
        {
            foreach (var coroutine in coroutines)
            {
                if (coroutine != null)
                {
                    controller.StopCoroutine(coroutine);
                }
            }
            coroutines.Clear();
        }
    }
    public enum EPlayerState
    {
        Idle,
        Running,
        Sliding,
        Wallsliding,
        Roll,
        Jumping,
        Falling,
        Hanging,
        Crouching,
        Climbing,
        AttackOne,
        AttackTwo,
        Hurt,
        CrouchAttack
    }
}
