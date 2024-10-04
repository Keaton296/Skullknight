using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerState
{
    [Header("PlayerController")]
    [SerializeField] PlayerController controller;

    public PlayerAttackingState(PlayerController controller) : base(controller)
    {
        this.controller = controller;
    }

    public override void OnStateEnd()
    {
        controller.currentActionCoroutine = null;
    }

    public override void OnStateStart()
    {
        // controller.swing0 = false;
        // controller.swing0Ended = false;
        // controller.swing1Triggered = false;
        // controller.swing1 = false;
        // controller.swing1Ended = false;
        //controller.currentActionCoroutine = controller.StartCoroutine(controller.AttackZero());
    }
    
    public override void StateUpdate()
    {
        // if(controller.swing0 && Input.GetMouseButtonDown(0))
        // {
        //     controller.swing1Triggered = true;
        // }
        
    } 
}
