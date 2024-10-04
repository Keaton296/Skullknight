using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class DemonFirstPhase : DemonState
{
    [Space]
    [SerializeField] Animator breathAnimator;
    [SerializeField] Collider2D playerOnFloorChecker;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] float inBetweenWaitDuration = 2;

    [SerializeField] bool breathAttack0 = false;
    [SerializeField] bool breathAttack1 = false;
    [SerializeField] bool breathAttack2 = false;

    [SerializeField] bool fireballAttack0 = false;
    [SerializeField] bool fireballAttack1 = false;
    [SerializeField] bool fireballAttack2 = false;
    [SerializeField] bool fireballAttack3 = false;

    [SerializeField] GameObject fireballObject;
    [SerializeField] Transform firePoint;
    Coroutine moveMakerCoroutine;

    public DemonFirstPhase(DemonBossController controller) : base(controller)
    {
        this.controller = controller;
    }

    public override void OnStateStart()
    {
        moveMakerCoroutine = controller.StartCoroutine(MoveMaker(controller));
    }

    public override void OnStateEnd()
    {

    }

    public override void StateFixedUpdate()
    {
        if (controller.canMove)
        {
            if(controller.moveTarget == controller.playerTransform) controller.transform.position = Vector2.Lerp(controller.transform.position, (Vector2)controller.moveTarget.position + controller.fireBreathDemonOffset, controller.moveLerpSpeed);
            else controller.transform.position = Vector2.Lerp(controller.transform.position, (Vector2)controller.moveTarget.position, controller.moveLerpSpeed);
        }
        if(controller.Health <= 0)
        {
            controller.StopAllCoroutines();
            controller.animator.SetTrigger("idle");
            controller.DemonState = controller.DemonSecondPhase;
        }
    }

    public override void StateUpdate()
    {
        if (controller.playerTransform.position.x - controller.transform.position.x > 0 && controller.canTurn)
        {
            controller.spriteRenderer.flipX = true;
        }
        else
        {
            controller.spriteRenderer.flipX = false;
        }
    }

    public override bool IsAccessible()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator MoveMaker(DemonBossController controller)
    {
        while (true)
        {
            if (Physics2D.BoxCast(playerOnFloorChecker.bounds.center, playerOnFloorChecker.bounds.size, 0f, Vector2.zero, 0f, playerLayerMask).transform != null)
            {
                controller.currentMove = controller.StartCoroutine(BreathAttack(controller));
            }
            else
            {
                controller.currentMove = controller.StartCoroutine(FireBallAttack(controller));
            }
            yield return new WaitUntil(() => controller.currentMove == null);
            yield return new WaitForSeconds(inBetweenWaitDuration);
            controller.currentMove = controller.StartCoroutine(controller.GoToPlace(controller,controller.flyingPlaceTransform));
            yield return new WaitUntil(() => controller.currentMove == null);
            yield return new WaitForSeconds(inBetweenWaitDuration);
        }
    }
    IEnumerator BreathAttack(DemonBossController controller)
    {
        controller.animator.SetTrigger("breath");
        controller.canMove = false;
        controller.canTurn = true;
        yield return new WaitUntil(() => breathAttack0);
        controller.moveTarget = controller.playerTransform;
        controller.canMove = true;
        controller.canTurn = true;
        yield return new WaitUntil(() => breathAttack1);
        controller.vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        controller.moveTarget = null;
        controller.canMove = false;
        controller.canTurn = false;
        breathAnimator.SetTrigger("red");
        yield return new WaitUntil(() => breathAttack2);
        controller.vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        breathAttack0 = false;
        breathAttack1 = false;
        breathAttack2 = false;
        controller.canTurn = true;
        controller.currentMove = null;
    }
    IEnumerator FireBallAttack(DemonBossController controller)
    {
        controller.animator.SetTrigger("fireball");
        yield return new WaitUntil(() => fireballAttack0);
        ShootFireball(controller.playerTransform);
        yield return new WaitUntil(() => fireballAttack1);
        ShootFireball(controller.playerTransform);
        yield return new WaitUntil(() => fireballAttack2);
        ShootFireball(controller.playerTransform);
        yield return new WaitUntil(() => fireballAttack3);
        fireballAttack0 = false;
        fireballAttack1 = false;
        fireballAttack2 = false;
        fireballAttack3 = false;
        controller.currentMove = null;
    }
    void ShootFireball(Transform target)
    {
        Fireball script = GameObject.Instantiate(fireballObject,firePoint.position,Quaternion.identity).GetComponent<Fireball>();
        script.SetFireball(target);
    }
    public void SetBreathAttackCheckTrue(int index)
    {
        switch (index)
        {
            case 0:
                breathAttack0 = true;
                break;
            case 1:
                breathAttack1 = true;
                break;
            case 2:
                breathAttack2 = true;
                break;
            default:
                break;
        }
        
    }
    public void SetFireballAttackCheckTrue(int index)
    {
        switch (index)
        {
            case 0:
                fireballAttack0 = true;
                break;
            case 1:
                fireballAttack1 = true;
                break;
            case 2:
                fireballAttack2 = true;
                break;
            case 3:
                fireballAttack3 = true;
                break;
            default:
                break;
        }

    }

    
}
