using UniRx;

namespace Assets.Scripts.Runtime.Shared.Interfaces
{
    public interface IGoalManager
    {
        int CurrentScore { get; }
        int FireballThreshold { get; }
        int FireballStreak {  get; }

        void ShowFireballBar(bool show);
    }
}