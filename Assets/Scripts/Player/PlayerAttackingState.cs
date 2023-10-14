using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : MonoBehaviour,IState
{
    [Header("PlayerController")]
    [SerializeField] PlayerController controller;
    public void OnStateEnd()
    {
        controller.currentActionCoroutine = null;
    }

    public void OnStateStart()
    {
        controller.swing0 = false;
        controller.swing0Ended = false;
        controller.swing1Triggered = false;
        controller.swing1 = false;
        controller.swing1Ended = false;
        controller.currentActionCoroutine = StartCoroutine(controller.AttackZero());
    }

    public void StateFixedUpdate()
    {
        
    }

    public void StateUpdate()
    {
        if(controller.swing0 && Input.GetMouseButtonDown(0))
        {
            controller.swing1Triggered = true;
        }
        
    }
}
