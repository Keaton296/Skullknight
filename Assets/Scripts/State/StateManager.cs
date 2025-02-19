using System;
using System.Collections.Generic;
using Player.Statemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Skullknight.State
{
    public abstract class StateManager<EState,State> : MonoBehaviour where EState : Enum where State : BaseState<EState>
    {
        protected EState stateEnum;
        public EState StateEnum => stateEnum;
        protected State currentState;
        protected Dictionary<EState, State> states = new Dictionary<EState, State>();
        
        [HideInInspector] public UnityEvent<EState> OnStateChange;
        
        protected virtual void Update()
        {
            currentState?.StateUpdate();
        }

        protected virtual void FixedUpdate()
        {
            currentState?.StateFixedUpdate();
        }

        public virtual void ChangeState(EState newState) 
        {
            if (states.ContainsKey(newState))
            {
                currentState?.ExitState();
                currentState = states[newState];
                stateEnum = newState;
                OnStateChange?.Invoke(stateEnum);
                currentState.EnterState();
            }
            else
            {
                Debug.LogError(string.Format("State '{0}' not found", newState));
            }
        }
    }
}