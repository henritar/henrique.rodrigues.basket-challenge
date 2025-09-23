using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UniRx;

namespace Assets.Scripts.Runtime.UI.GameplayUI
{
    public class GameplayUIModel : BaseModel, IGameplayUIModel
    {
        private readonly ReactiveProperty<bool> _isUIVisible = new ReactiveProperty<bool>(false);

        private readonly ReactiveProperty<int> _playerCurrentPoints = new ReactiveProperty<int>();
        private readonly ReactiveProperty<int> _npcCurrentPoints = new ReactiveProperty<int>();

        public IReadOnlyReactiveProperty<bool> IsUIVisible => _isUIVisible;
        public IReadOnlyReactiveProperty<int> PlayerCurrentPoints => _playerCurrentPoints;
        public IReadOnlyReactiveProperty<int> NpcCurrentPoints => _npcCurrentPoints;

        public void SetUIVisible(bool visible)
        {
            _isUIVisible.Value = visible;
        }

        public void UpdatePlayerPoints(int points)
        {
            _playerCurrentPoints.Value = points;
        }

        public void UpdateNpcPoints(int points)
        {
            _npcCurrentPoints.Value = points;
        }
    }

}