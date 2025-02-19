using System;
using UnityEngine;

namespace Skullknight.State
{
    public abstract class EntityStateManager<EState,State,Controller> : StateManager<EState,State> where EState : Enum where Controller : MonoBehaviour where State : EntityState<EState,Controller>
    {
        public override void ChangeState(EState newState) 
        {
            if (states.ContainsKey(newState))
            {
                currentState?.UnsubscribeEvents();
                EntityState<EState,Controller> dState = currentState as EntityState<EState,Controller>;
                dState?.KillCoroutines();
                currentState?.ExitState();
                currentState = states[newState];
                stateEnum = newState;
                OnStateChange?.Invoke(stateEnum);
                currentState.EnterState();
                currentState.SubscribeEvents();
            }
            else
            {
                Debug.LogError(string.Format("State '{0}' not found", newState));
            }
        }
        protected virtual void OnEnable()
        {
            currentState?.SubscribeEvents();
        }

        protected virtual void OnDisable()
        {
            currentState?.UnsubscribeEvents();
            currentState?.KillCoroutines();
        }
    }
}