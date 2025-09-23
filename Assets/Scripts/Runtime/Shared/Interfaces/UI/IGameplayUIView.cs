using Assets.Scripts.Runtime.Shared.Interfaces.MVP;

namespace Assets.Scripts.Runtime.Shared.Interfaces.UI
{
    public interface IGameplayUIView : IBaseView
    {
        void UpdatePlayerScore(int score);
        void UpdateNpcScore(int score);
    }
}