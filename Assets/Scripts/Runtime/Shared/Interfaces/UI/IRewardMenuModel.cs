using Assets.Scripts.Runtime.Shared.Interfaces.MVP;
using UniRx;

namespace Assets.Scripts.Runtime.Shared.Interfaces.UI
{
    public interface IRewardMenuModel : IBaseModel
    {
        IReadOnlyReactiveProperty<bool> IsUIVisible { get; }
        IReadOnlyReactiveProperty<int> PlayerFinalScore { get; }
        IReadOnlyReactiveProperty<int> NpcFinalScore { get; }
        void SetUIVisible(bool visible);
        void SetPlayerFinalScore(int finalScore);
        void SetNpcFinalScore(int finalScore);
    }
}