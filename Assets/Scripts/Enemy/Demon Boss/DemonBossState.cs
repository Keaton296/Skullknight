using System.Collections.Generic;
using Skullknight.State;
using UnityEngine;

public abstract class DemonBossState : BaseState<EDemonBossState>
{
    protected DemonBossController controller;
    protected List<Coroutine> coroutines; 

    public DemonBossState(DemonBossController _controller)
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
}