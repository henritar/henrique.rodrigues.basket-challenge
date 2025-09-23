using Assets.Scripts.Runtime.Shared.Interfaces.MVP;
using UnityEngine.Events;

namespace Assets.Scripts.Runtime.Shared.Interfaces.UI
{
    public interface IRewardMenuPresenter : IBasePresenter
    {
        void SetPlayAgainAction(UnityAction action);
        void SetMainMenuAction(UnityAction action);
        void ShowUI(bool show);
        void SetPlayerFinalScore(int finalScore);
        void SetNpcFinalScore(int finalScore);
    }
}