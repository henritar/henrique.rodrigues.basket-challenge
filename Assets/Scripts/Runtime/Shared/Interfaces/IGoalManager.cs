using UniRx;

namespace Assets.Scripts.Runtime.Shared.Interfaces
{
    public interface IGoalManager : IBaseManager
    {
        int PlayerCurrentScore { get; }
        int NpcCurrentScore { get; }
        int FireballThreshold { get; }
        int FireballStreak {  get; }

        void ShowFireballBar(bool show);
    }
}