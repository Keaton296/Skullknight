using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Skullknight
{
    public class CameraManager : MonoBehaviour
    {
        private int selectionPriorityValueBuffer;
        private const int SELECTION_PRIORITY = 100;
        private int selectedCameraIndex = 0;
        
        [SerializeField] private CinemachineVirtualCamera[] cameras;

        void OnEnable()
        {
            GameManager.Instance.OnBossChange.AddListener(OnBossfightToggle);
        }

        void OnDisable()
        {
            GameManager.Instance.OnBossChange.RemoveListener(OnBossfightToggle);
        }

        void Start()
        {
            SelectCamera(0);
        }

        private void SelectCamera(int cameraIndex)
        {
            if (cameraIndex < 0 || cameraIndex >= cameras.Length) return;
            cameras[selectedCameraIndex].Priority = selectionPriorityValueBuffer;
            selectionPriorityValueBuffer = cameras[cameraIndex].Priority;
            selectedCameraIndex = cameraIndex;
            cameras[selectedCameraIndex].Priority = SELECTION_PRIORITY;
        }
        private void OnBossfightToggle(GameObject boss)
        {
            if (boss != null)
            {
                SelectCamera(1);
            }
            else
            {
                SelectCamera(0);
            }
        }
    }
}
