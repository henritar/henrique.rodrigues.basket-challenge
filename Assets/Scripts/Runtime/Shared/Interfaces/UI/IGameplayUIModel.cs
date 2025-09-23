using Assets.Scripts.Runtime.Shared.Interfaces.MVP;
using UniRx;

namespace Assets.Scripts.Runtime.Shared.Interfaces.UI
{
    public interface IGameplayUIModel : IBaseModel
    {
        IReadOnlyReactiveProperty<int> PlayerCurrentPoints { get; }
        IReadOnlyReactiveProperty<int> NpcCurrentPoints { get; }
        IReadOnlyReactiveProperty<bool> IsUIVisible { get; }
        void UpdatePlayerPoints(int points);
        void UpdateNpcPoints(int points);
        void SetUIVisible(bool visible);

    }
}