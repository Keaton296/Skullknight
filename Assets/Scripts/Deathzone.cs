using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionChecker))]
public class Deathzone : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<CollisionChecker>().onTriggerEnter2D += OnDeathZoneTriggerEnter;
    }
    void OnDeathZoneTriggerEnter(Collider2D player)
    {
        if(GameManager.Instance.checkPointLoadingCoroutine == null) GameManager.Instance.checkPointLoadingCoroutine = GameManager.Instance.StartCoroutine(GameManager.Instance.LoadLastCheckpoint());
    }
}
