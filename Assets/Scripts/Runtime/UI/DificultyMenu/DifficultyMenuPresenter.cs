using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UniRx;

namespace Assets.Scripts.Runtime.UI.DificultyMenu
{
    public class DifficultyMenuPresenter : BasePresenter<IDifficultyMenuModel, IDifficultyMenuView>, IDifficultyMenuPresenter
    {
        private readonly INpcManager _npcManager;
        private readonly INpcConfigData _npcConfigData;
        private CompositeDisposable _disposables = new CompositeDisposable();

        public DifficultyMenuPresenter(IDifficultyMenuModel model, IDifficultyMenuView view, INpcManager npcManager, INpcConfigData npcConfigData) : base(model, view)
        {
            _npcManager = npcManager;
            _npcConfigData = npcConfigData;
        }

        public void ShowUI(bool show)
        {
            Model.SetUIVisible(show);
        }

        protected override void SubscribeToEvents()
        {
            Model.IsUIVisible.Subscribe(OnUIVisibleChanged).AddTo(_disposables);
            View.OnDifficultyConfigChanged.Subscribe(OnConfigValueChanged).AddTo(_disposables);

            View.SetDifficultyValues(_npcConfigData.NpcDifficultyConfigs);
        }

        protected override void UnsubscribeFromEvents()
        {
        }

        protected override void Cleanup()
        {
            _disposables.Dispose();
            _disposables = null;
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

        private void OnConfigValueChanged(NpcDifficultyConfig value)
        {
            Model.NpcDifficulty = value;
            _npcManager.SetDifficultConfig(value);
        }
    }
}