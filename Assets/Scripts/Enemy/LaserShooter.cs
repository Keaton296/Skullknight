using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class LaserShooter : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    
    [SerializeField] LineRenderer laserRenderer;
    [SerializeField] private Animator laserAnimator;

    [SerializeField] private LayerMask laserStoppingMask;
    [SerializeField] private CinemachineImpulseSource impulseSrc;
    
    Coroutine fireCoroutine;
    public void ShootTarget(Transform _target)
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(Shoot(_target));
    }
    IEnumerator Shoot(Transform _target)
    {
        Vector2 direction = _target.position - shootPoint.position;
        direction.Normalize();
        var hit = Physics2D.Raycast(shootPoint.position,direction,50f,laserStoppingMask);
        laserRenderer.SetPositions(new Vector3[] { shootPoint.position, hit.point });
        impulseSrc.GenerateImpulse(new Vector3(Random.Range(0f,1f), Random.Range(0f,1f), 0));
        laserAnimator.Play("Laser");
        DOTween.To(() => laserRenderer.widthMultiplier, x => laserRenderer.widthMultiplier = x, 0f, laserAnimator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(laserAnimator.GetCurrentAnimatorStateInfo(0).length);
        yield return null;
        laserRenderer.widthMultiplier = 1f;
        fireCoroutine = null;
    }
   
}
