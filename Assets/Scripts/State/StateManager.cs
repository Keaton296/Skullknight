using System;
using System.Collections.Generic;
using Player.Statemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Skullknight.State
{
    public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
    {
        protected EState stateEnum;
        public EState StateEnum => stateEnum;
        protected BaseState<EState> currentState;
        protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();
        
        [HideInInspector] public UnityEvent<EState> OnStateChange;

        abstract protected void Awake();
        abstract protected void Start();

        protected virtual void Update()
        {
            currentState?.StateUpdate();
        }

        protected virtual void FixedUpdate()
        {
            currentState?.StateFixedUpdate();
        }

        public void ChangeState(EState newState) 
        {
            if (states.ContainsKey(newState))
            {
                currentState?.UnsubscribeEvents();
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
        }
    }
}