using UnityEngine;

namespace Player.Statemachine
{
    public abstract class PlayerState 
    {
        [SerializeField] protected PlayerController controller;
        public PlayerState(PlayerController controller)
        {
            controller = this.controller;
        }

        public virtual void StateFixedUpdate()
        {
        
        }
        public abstract void OnStateStart();
        public abstract void OnStateEnd();
        public abstract void StateUpdate();
    }
}
