using Assets.Scripts.Runtime.Shared.Interfaces.MVP;
using System;

namespace Assets.Scripts.Runtime.Shared.Interfaces.UI
{
    public interface IDifficultyMenuView : IBaseView
    {
        void SetDifficultyValues(NpcDifficultyConfig[] values);
        IObservable<NpcDifficultyConfig> OnDifficultyConfigChanged { get; }
    }
}