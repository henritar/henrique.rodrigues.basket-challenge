using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared.Interfaces.MVP;
using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.Shared.Interfaces.Interactables
{
    public interface IBallPresenter : IBasePresenter
    {
        Transform BallTransform { get; }
        PlayerTypeEnum BallPlayerType { get; }
        Vector3 BallPosition { get; set; }
        IObservable<Unit> OnBallReset { get; }
        void SetBallVelocity(Vector3 velocity);
        void ResetBall();
    }
}