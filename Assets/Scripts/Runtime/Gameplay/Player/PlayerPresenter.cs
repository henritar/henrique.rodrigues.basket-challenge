using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.Gameplay.Player
{
    public class PlayerPresenter : BasePresenter<IPlayerModel, IPlayerView>, IPlayerPresenter
    {
        private CompositeDisposable _disposables;

        public PlayerPresenter(IPlayerModel model, IPlayerView view) : base(model, view)
        {
            Initialize();
        }

        public IBallPresenter GetBall() => Model.BallPresenter;

        public void MoveToPosition(Vector3 position)
        {
            Model.SetPosition(position);
        }

        protected override void SubscribeToEvents()
        {
            Model.CurrentPosition
                .Subscribe(pos => View.SetPosition(pos)).AddTo(_disposables);
        }

        protected override void UnsubscribeFromEvents()
        {
        }

        protected override void OnInitialize()
        {
            _disposables = new CompositeDisposable();
            Model.SetPosition(View.CurrentPosition);
        }

        protected override void Cleanup()
        {
            _disposables.Dispose();
            _disposables = null;
        }
    }
}