using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skullknight.State
{
    public abstract class BaseState<EState> where EState : Enum
    {
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void StateUpdate();
        public abstract void StateFixedUpdate();
        public abstract void SubscribeEvents();
        public abstract void UnsubscribeEvents();
    }
}