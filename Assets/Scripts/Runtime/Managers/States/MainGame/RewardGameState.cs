using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UnityEngine;

namespace Assets.Scripts.Runtime.Managers.States.MainGame
{
    public class RewardGameState : BaseGameState
    {
        private readonly IRewardMenuPresenter _rewardMenuPresenter;
        private readonly IGoalManager _goalManager;
        protected override GameStatesEnum GameState => GameStatesEnum.Reward;

        public RewardGameState(IGoalManager goalManager, IRewardMenuPresenter rewardMenuPresenter)
        {
            _goalManager = goalManager;
            _rewardMenuPresenter = rewardMenuPresenter;
            _rewardMenuPresenter.SetMainMenuAction(() =>
            {
                _stateManager.ChangeState(GameStatesEnum.MainMenu);
            });
            _rewardMenuPresenter.SetPlayAgainAction(() =>
            {
                _stateManager.ChangeState(GameStatesEnum.Playing);
            });
        }

        protected override void OnEnterState()
        {
            Debug.Log("Entering Reward Game State");
            _rewardMenuPresenter.ShowUI(true);
            _rewardMenuPresenter.SetPlayerFinalScore(_goalManager.PlayerCurrentScore);
            _rewardMenuPresenter.SetNpcFinalScore(_goalManager.NpcCurrentScore);
        }

        protected override void OnExitState()
        {
            Debug.Log("Exiting Reward Game State");
            _rewardMenuPresenter.ShowUI(false);
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnFixedUpdate()
        {
        }
    }
}