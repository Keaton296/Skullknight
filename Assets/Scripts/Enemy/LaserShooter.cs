using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;
using Skullknight.Core;
using Skullknight.Player.Statemachine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LaserShooter : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    
    [SerializeField] LineRenderer laserRenderer;
    [SerializeField] private Animator laserAnimator;

    [SerializeField] private LayerMask laserStoppingMask;
    [SerializeField] private LayerMask targetMask;
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
        var cast = Physics2D.BoxCastAll(
            new Vector2(shootPoint.position.x + hit.point.x,shootPoint.position.y + hit.point.y)/2f,
            new Vector2((hit.point - (Vector2)shootPoint.position).magnitude, 1f),
            Vector2.SignedAngle(Vector2.right,hit.point - (Vector2)shootPoint.position),
            Vector2.zero);
        foreach (var item in cast)
        {
            if (item.collider != null && !item.collider.isTrigger)
            {
                var damageable = item.collider.GetComponent<IDamageable>();
                if (damageable != null && damageable.IsDamageable)
                {
                    damageable.TakeDamage(10);
                }
            }
        }
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
