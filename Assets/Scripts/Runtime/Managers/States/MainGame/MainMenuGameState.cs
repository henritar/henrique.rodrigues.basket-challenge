using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UnityEngine;

namespace Assets.Scripts.Runtime.Managers.States.MainGame
{
    public class MainMenuGameState : BaseGameState
    {
        protected override GameStatesEnum GameState => GameStatesEnum.MainMenu;

        private readonly IMainMenuPresenter _mainMenuPresenter;
        private readonly ITimerMenuPresenter _timerMenuPresenter;
        private readonly IDifficultyMenuPresenter _difficultyMenuPresenter;

        public MainMenuGameState(IMainMenuPresenter mainMenuPresenter, ITimerMenuPresenter timerMenuPresenter, IDifficultyMenuPresenter difficultyMenuPresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
            _timerMenuPresenter = timerMenuPresenter;
            _difficultyMenuPresenter = difficultyMenuPresenter;
            _mainMenuPresenter.SetStartGameAction(() =>
            {
                _stateManager.ChangeState(GameStatesEnum.Playing);
            });
        }

        protected override void OnEnterState()
        {
            Debug.Log("Entering MainMenu Game State");
            _mainMenuPresenter.ShowUI(true);
            _timerMenuPresenter.ShowUI(true);
            _difficultyMenuPresenter.ShowUI(true);
        }

        protected override void OnExitState()
        {
            _mainMenuPresenter.ShowUI(false);
            _timerMenuPresenter.ShowUI(false);
            _difficultyMenuPresenter.ShowUI(false);
            Debug.Log("Exiting MainMenu Game State");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnFixedUpdate()
        {
        }
    }
}
