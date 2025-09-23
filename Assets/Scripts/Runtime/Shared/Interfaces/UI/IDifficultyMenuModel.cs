using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared.Interfaces.MVP;
using UniRx;

namespace Assets.Scripts.Runtime.Shared.Interfaces.UI
{
    public interface IDifficultyMenuModel : IBaseModel
    {
        NpcDifficultyConfig NpcDifficulty { get; set; }
        IReadOnlyReactiveProperty<bool> IsUIVisible { get; }
        void SetUIVisible(bool visible);
    }
}