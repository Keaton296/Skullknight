using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFunctionsHolder : MonoBehaviour
{
    [Header("PlayerController")]
    [SerializeField] PlayerController controller;
    public void SetAnimationPointTrue(int index)
    {
        switch (index)
        {
            case 0:
                controller.swing0 = true;
                break;
            case 1:
                controller.swing0Ended = true;
                break;
            case 2:
                controller.swing1 = true;
                break;
            case 3:
                controller.swing1Ended = true;
                break;
            case 4:
                controller.climbAnimEnded = true;
                break;
            default:
                break;
        }
    }

}
