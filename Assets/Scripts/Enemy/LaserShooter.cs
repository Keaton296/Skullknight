using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserShooter : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] LineRenderer laserRenderer;
    [SerializeField] float chargeTimeSeconds = 4f;
    Coroutine chargeCoroutine;
    private void Start()
    {
        
        chargeCoroutine = null;
        ShootTarget(target);
    }
    public void ShootTarget(Transform _target)
    {
        if (chargeCoroutine == null) chargeCoroutine = StartCoroutine(Shoot(_target));
    }
    public void ShootPoint()
    {

    }
    IEnumerator Shoot(Transform _target)
    {
        target = _target;
        this.enabled = true;
        laserRenderer.enabled = true;
        laserRenderer.widthMultiplier = 1f;
        DOTween.To(() => laserRenderer.widthMultiplier, x => laserRenderer.widthMultiplier = x, 0f, chargeTimeSeconds);
        yield return new WaitForSeconds(chargeTimeSeconds);
        laserRenderer.enabled = false;
        this.enabled = false;
        chargeCoroutine = null;
    }
    private void Update()
    {
        laserRenderer.SetPositions(new Vector3[] { transform.position, target.position });
    }
}
