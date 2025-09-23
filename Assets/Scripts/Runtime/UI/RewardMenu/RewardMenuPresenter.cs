using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UniRx;
using UnityEngine.Events;

namespace Assets.Scripts.Runtime.UI.RewardMenu
{
    public class RewardMenuPresenter : BasePresenter<IRewardMenuModel, IRewardMenuView>, IRewardMenuPresenter
    {
        private CompositeDisposable _disposables = new CompositeDisposable();

        public RewardMenuPresenter(IRewardMenuModel model, IRewardMenuView view) : base(model, view)
        {
        }

        public void ShowUI(bool show)
        {
            Model.SetUIVisible(show);
        }

        public void SetMainMenuAction(UnityAction action)
        {
            View.SetMainMenuAction(action);
        }

        public void SetPlayAgainAction(UnityAction action)
        {
            View.SetPlayAgainAction(action);
        }

        public void SetPlayerFinalScore(int finalScore)
        {
            Model.SetPlayerFinalScore(finalScore);
        }
        public void SetNpcFinalScore(int finalScore)
        {
            Model.SetNpcFinalScore(finalScore);
        }

        protected override void SubscribeToEvents()
        {

            Model.IsUIVisible.Subscribe(OnUIVisibleChanged).AddTo(_disposables);
            Model.PlayerFinalScore.Subscribe(View.SetPlayerFinalScore).AddTo(_disposables);
            Model.NpcFinalScore.Subscribe(View.SetNpcFinalScore).AddTo(_disposables);
        }

        protected override void UnsubscribeFromEvents()
        {
        }

        private void OnUIVisibleChanged(bool visible)
        {
            switch (visible)
            {
                case true:
                    View.Show();
                    break;
                case false:
                    View.Hide();
                    break;
            }
        }

        protected override void Cleanup()
        {
            _disposables.Dispose();
            _disposables = null;
        }

    }
}