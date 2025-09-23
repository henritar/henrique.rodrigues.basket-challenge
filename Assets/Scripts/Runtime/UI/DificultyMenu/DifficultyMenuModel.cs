using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UniRx;

namespace Assets.Scripts.Runtime.UI.DificultyMenu
{
    public class DifficultyMenuModel : BaseModel, IDifficultyMenuModel
    {
        private readonly ReactiveProperty<bool> _isUIVisible = new ReactiveProperty<bool>(false);
        public NpcDifficultyConfig NpcDifficulty { get; set; }
        public IReadOnlyReactiveProperty<bool> IsUIVisible => _isUIVisible;

        public void SetUIVisible(bool visible)
        {
            _isUIVisible.Value = visible;
        }
    }
}