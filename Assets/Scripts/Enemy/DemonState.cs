using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DemonState
{
    protected DemonBossController controller;

    public DemonState(DemonBossController controller)
    {
        this.controller = controller;
    }
    public void SetFlip(bool value)
    {
        //attackHitbox.localScale = new Vector3(value != spriteRenderer.flipX ? -attackHitbox.localScale.x : attackHitbox.localScale.x,attackHitbox.localScale.y,attackHitbox.localScale.z);
        //landFXPlayerPoint.localPosition = new Vector3(value != spriteRenderer.flipX ? -landFXPlayerPoint.localPosition.x : landFXPlayerPoint.localPosition.x, landFXPlayerPoint.localPosition.y, landFXPlayerPoint.localPosition.z);
        //landFXRenderer.flipX = value;
        controller.spriteRenderer.flipX = value;
        //wallSlideParticleTransform.localPosition = new Vector3(value ? .539f : -.539f, wallSlideParticleTransform.localPosition.y);
    }
    public abstract void OnStateStart();
    public abstract void OnStateEnd();
    public abstract void StateFixedUpdate();
    public abstract void StateUpdate();
    public abstract bool IsAccessible();
}
