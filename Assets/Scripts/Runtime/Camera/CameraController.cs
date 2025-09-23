using Assets.Scripts.Runtime.Shared.Interfaces;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Runtime.Camera
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [SerializeField] CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private float baseFOV = 60f;
        
        private float baseAspectRatio = 16f / 9f;

        public void SetCameraFollowTarget(Transform target)
        {
            _virtualCamera.Follow = target;
        }
        void Start()
        {
            AdjustFOV();
        }

        private void AdjustFOV()
        {
            float currentAspectRatio = (float)Screen.width / Screen.height;

            float fovAdjustment = (currentAspectRatio / baseAspectRatio);

            _virtualCamera.m_Lens.FieldOfView = baseFOV * (baseAspectRatio / currentAspectRatio);
        }
    }
}