using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDemonBossState 
{
    public void StateStart(DemonBossController controller);
    public void StateEnd(DemonBossController controller);
    public void StateUpdate(DemonBossController controller);
    public void StateFixedUpdate(DemonBossController controller);

}
