using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.Gameplay.Ball
{
    public class BallPresenter : BasePresenter<IBallModel, IBallView>, IBallPresenter
    {
        private readonly Subject<Unit> _onBallReset = new();
        private CompositeDisposable _disposables;
        public Vector3 BallPosition { get => Model.StartPosition.Value; set => Model.SetStartPosition(value); }
        public Transform BallTransform => View.Transform;
        public IObservable<Unit> OnBallReset => _onBallReset;

        public PlayerTypeEnum BallPlayerType => Model.PlayerType;
        public BallPresenter(IBallModel model, IBallView view) : base(model, view)
        {
            _disposables = new CompositeDisposable();
            View.SetPlayerType(BallPlayerType);

            View.ObserveEveryValueChanged(v => v.Transform.position.y).DistinctUntilChanged()
                .Where(y => y < 0.4f).Subscribe(_ => ResetBall()).AddTo(_disposables);

            Initialize();
        }

        public void SetBallVelocity(Vector3 velocity)
        {
            var rb = View.Rigidbody;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            rb.velocity = velocity;
            rb.isKinematic = false;
        }

        public void ResetBall()
        {
            var rb = View.Rigidbody;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
            ResetBallPosition();

            _onBallReset.OnNext(Unit.Default);
        }

        private void ResetBallPosition()
        {
            View.Transform.position = BallPosition;
        }

        protected override void SubscribeToEvents()
        {
            _disposables = new();
            Model.StartPosition.Subscribe(pos => View.Transform.position = pos).AddTo(_disposables);
        }

        protected override void UnsubscribeFromEvents()
        {

        }

        protected override void Cleanup()
        {
            _disposables.Dispose();
            _disposables = null;
        }
    }
}