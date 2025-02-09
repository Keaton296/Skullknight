using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skullknight
{
    public class CheckpointManager : MonoBehaviour
    {
        [SerializeField] private Transform lastCheckpoint;

        public void SetCheckpoint(Transform checkpoint)
        {
            lastCheckpoint = checkpoint;
        }
        public IEnumerator LoadLastCheckpoint()
        {
            /*PlayerController.Instance.enabled = false;
            cameraPostFX.DOFade(1f, .5f);
            yield return new WaitForSeconds(.5f);
            PlayerController.Instance.transform.position = currentCheckPoint.transform.position;
            yield return new WaitForSeconds(checkPointLoadSeconds);
            cameraPostFX.DOFade(0f, .5f);
            PlayerController.Instance.enabled = true;
            yield return new WaitForSeconds(.5f);
            checkPointLoadingCoroutine = null;*/
            yield return new WaitForSeconds(.5f);
        }
    }
}
