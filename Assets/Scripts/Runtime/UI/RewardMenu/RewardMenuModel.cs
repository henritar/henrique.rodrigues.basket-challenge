using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UniRx;

namespace Assets.Scripts.Runtime.UI.RewardMenu
{
    public class RewardMenuModel : BaseModel, IRewardMenuModel
    {
        private readonly ReactiveProperty<int> _playerFinalScore = new ReactiveProperty<int>();
        private readonly ReactiveProperty<int> _npcFinalScore = new ReactiveProperty<int>();
        private readonly ReactiveProperty<bool> _isUIVisible = new ReactiveProperty<bool>(false);
        public IReadOnlyReactiveProperty<bool> IsUIVisible => _isUIVisible;
        public IReadOnlyReactiveProperty<int> PlayerFinalScore => _playerFinalScore;
        public IReadOnlyReactiveProperty<int> NpcFinalScore => _npcFinalScore;

        public void SetUIVisible(bool visible)
        {
            _isUIVisible.Value = visible;
        }

        public void SetPlayerFinalScore(int finalScore)
        {
            _playerFinalScore.Value = finalScore;
        }
        public void SetNpcFinalScore(int finalScore)
        {
            _npcFinalScore.Value = finalScore;
        }
    }
}