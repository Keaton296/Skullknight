using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using DG.Tweening;
using Player.Statemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] PlayableDirector timelineDirector;
    [SerializeField] float checkPointLoadSeconds = 1f;
    [SerializeField] Image cameraPostFX;
    [Header("Checkpoints")]
    [SerializeField] Collider2D[] checkPoints;
    [SerializeField] Collider2D currentCheckPoint;

    public Coroutine checkPointLoadingCoroutine;
    private void Awake()
    {
        currentCheckPoint = checkPoints[0];
        Instance = this;
    }
    private void Start()
    {
        //StartCoroutine(FirstCinematic());
    }
    IEnumerator FirstCinematic()
    {
        PlayerController.Instance.enabled = false;
        timelineDirector.Play(timelineDirector.playableAsset);
        yield return new WaitForSeconds((float)timelineDirector.playableAsset.duration);
        PlayerController.Instance.enabled = true;
    }
    public IEnumerator LoadLastCheckpoint()
    {
        PlayerController.Instance.enabled = false;
        cameraPostFX.DOFade(1f, .5f);
        yield return new WaitForSeconds(.5f);
        PlayerController.Instance.transform.position = currentCheckPoint.transform.position;
        yield return new WaitForSeconds(checkPointLoadSeconds);
        cameraPostFX.DOFade(0f, .5f);
        PlayerController.Instance.enabled = true;
        yield return new WaitForSeconds(.5f);
        checkPointLoadingCoroutine = null;
    }

}
