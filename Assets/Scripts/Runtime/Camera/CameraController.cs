using Assets.Scripts.Runtime.Shared.Interfaces;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Runtime.Camera
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [SerializeField] CinemachineVirtualCamera _virtualCamera;

        public void SetCameraFollowTarget(Transform target)
        {
            _virtualCamera.Follow = target;
        }
    }
}