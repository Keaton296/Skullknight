using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonStateSecondPhase : MonoBehaviour, IState
{
    [Header("DemonBossController")]
    [SerializeField] DemonBossController controller;
    [Space]
    bool transformCry = false;
    bool transformCryEnd = false;
    public void OnStateStart()
    {
        transformCry = false;
        transformCryEnd = false;
        StartCoroutine(MoveMaker());
    }

    public void OnStateEnd()
    {

    }

    public void StateFixedUpdate()
    {

    }
    public void StateUpdate()
    {
        if (controller.canMove)
        {
            if (controller.moveTarget == controller.playerTransform) transform.position = Vector2.Lerp(transform.position, (Vector2)controller.moveTarget.position + controller.fireBreathDemonOffset, controller.moveLerpSpeed);
            else transform.position = Vector2.Lerp(transform.position, (Vector2)controller.moveTarget.position, controller.moveLerpSpeed);
        }
    }
    IEnumerator PhaseTransform()
    {
        controller.animator.SetTrigger("transform");
        controller.vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 2;
        DOTween.To(()=>controller.spriteRenderer.material.GetFloat("_EmissionPercent1"),x=> controller.spriteRenderer.material.SetFloat("_EmissionPercent1",x),4f,1.1f/12).SetEase(Ease.OutSine);
        yield return new WaitUntil(()=>transformCry);
        controller.vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        controller.Health = 300;
        controller.floorFireAnimator.SetTrigger("start");
    }
    IEnumerator MoveMaker()
    {
        Debug.Log("start");
        controller.currentMove = StartCoroutine(controller.GoToPlace(controller, controller.flyingPlaceTransform));
        yield return new WaitUntil(()=> controller.currentMove == null);
        Debug.Log("end");
        controller.currentMove = StartCoroutine(PhaseTransform());
        yield return new WaitUntil(()=> controller.currentMove == null);

    }
    public void SetAnimationCheckTrue(int index)
    {
        switch (index)
        {
            case 0:
                transformCry = true;
                break;
            case 1:
                transformCryEnd = true;
                break;
            default:
                break;
        }
    }
}
