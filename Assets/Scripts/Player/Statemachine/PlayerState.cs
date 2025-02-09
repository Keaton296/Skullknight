using Skullknight.Player.Statemachine;
using Skullknight.State;
using UnityEngine;

namespace Player.Statemachine
{
    public abstract class PlayerState : BaseState<EPlayerState>
    {
        protected PlayerController controller;
        public PlayerState(PlayerController playerController)
        {
            controller = playerController;
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
        AttackTwo
    }
}
