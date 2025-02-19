using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skullknight.State
{
    public abstract class EntityState<EState,Controller> : BaseState<EState> where EState : Enum where Controller : MonoBehaviour
    {
        protected Controller controller;
        protected List<Coroutine> coroutines;
        
        public EntityState(Controller _controller)
        {
            controller = _controller;
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
        public abstract void SubscribeEvents();
        public abstract void UnsubscribeEvents();
    }
}