using UnityEngine;

namespace Assets.Scripts.Runtime.Shared.Interfaces
{
    public interface ICameraController
    {
        void SetCameraFollowTarget(Transform target);
    }
}