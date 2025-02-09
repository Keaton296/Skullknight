using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Random = UnityEngine.Random;

public class DemonIdleState : DemonBossState
{
    public DemonIdleState(DemonBossController _controller) : base(_controller)
    { }

    public override void EnterState()
    {
        controller.animator.Play("Idle");
        controller.canTakeDamage = true;
        controller.StartCoroutine(PrepareNextAction());
    }

    private IEnumerator PrepareNextAction()
    {
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        switch (controller.Phase)
        {
            case DemonBossController.DemonBossPhase.FirstPhase:
                float roll = Random.Range(0f, 1f);
                if (roll > .5f)
                {
                    controller.ChangeState(EDemonBossState.BreathAttack);
                }
                else
                {
                    controller.ChangeState(EDemonBossState.FireballAttack);
                }
                break;
            case DemonBossController.DemonBossPhase.SecondPhase:
                break;
            default:
                break;
        }
    }
    public override void ExitState()
    {
        
    }

    public override void StateUpdate()
    {
        controller.LookPlayer();
    }

    public override void StateFixedUpdate()
    {
        
    }

    public override void SubscribeEvents()
    {
        
    }

    public override void UnsubscribeEvents()
    {
        
    }
}
