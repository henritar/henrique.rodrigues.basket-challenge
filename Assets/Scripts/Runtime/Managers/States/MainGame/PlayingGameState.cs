using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.InputSystem.Gameplay;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.Managers.States.MainGame
{
    public class PlayingGameState : BaseGameState
    {
        private readonly IPlayingInputHandler _inputHandler;
        private readonly IEventBus _eventBus;
        private readonly IBallPresenter _ballPresenter;
        private readonly IGameplayUIPresenter _gameplayUIPresenter;
        private readonly ITimerUIPresenter _timerUIPresenter;
        private readonly IGameplayInputManager _inputManager;
        private readonly ISwipeManager _swipeManager;
        private readonly IBackboardBonusManager _backboardBonusManager;
        private readonly ITimerManager _timerManager;
        private readonly IGoalManager _goalManager;

        private CompositeDisposable _disposables;

        protected override GameStatesEnum GameState => GameStatesEnum.Playing;

        public PlayingGameState(IPlayingInputHandler inputHandler, IEventBus eventBus,
            IBallPresenter ballPresenter, IGameplayUIPresenter gameplayUIPresenter, ITimerUIPresenter timerUIPresenter,
            IGameplayInputManager inputManager, ISwipeManager swipeManager, IBackboardBonusManager backboardBonusManager,
            ITimerManager timerManager, IGoalManager goalManager)
        {
            _inputHandler = inputHandler;
            _eventBus = eventBus;
            _ballPresenter = ballPresenter;
            _gameplayUIPresenter = gameplayUIPresenter;
            _timerUIPresenter = timerUIPresenter;
            _inputManager = inputManager;
            _swipeManager = swipeManager;
            _backboardBonusManager = backboardBonusManager;
            _timerManager = timerManager;
            _goalManager = goalManager;

            _inputHandler.BallPresenter = _ballPresenter;
        }

        protected override void OnEnterState()
        {
            Debug.Log("Entering Playing Game State");

            _inputManager.SetCurrentInputHandler(_inputHandler);

            _backboardBonusManager.StartBonusGeneration();
            _timerManager.StartTimer();
            _swipeManager.ResetSwipeTracking();
            _swipeManager.ShowInputBar(true);
            _goalManager.ShowFireballBar(true);

            _gameplayUIPresenter.ShowUI(true);
            _timerUIPresenter.ShowUI(true);

            SubscribeToEvents();

            _eventBus.Publish(new GameStartEvent());
        }

        protected override void OnExitState()
        {
            UnsubscribeFromEvents();

            _timerUIPresenter.ShowUI(false);
            _gameplayUIPresenter.ShowUI(false);
            _swipeManager.ShowInputBar(false);
            _goalManager.ShowFireballBar(false);

            _backboardBonusManager.StopBonusGeneration();


            _inputManager.SetCurrentInputHandler(null);
            
            Debug.Log("Exiting Playing Game State");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnFixedUpdate()
        {
        }

        protected void UnsubscribeFromEvents()
        {
            _inputHandler.OnHoldClick -= _swipeManager.StartSwipeTracking;
            _inputHandler.OnReleaseClick -= _swipeManager.EndSwipeTracking;
            
            _disposables.Dispose();
            _disposables.Clear();
        }

        private void SubscribeToEvents()
        {
            _inputHandler.OnHoldClick += _swipeManager.StartSwipeTracking;
            _inputHandler.OnReleaseClick += _swipeManager.EndSwipeTracking;

            _disposables = new CompositeDisposable();

            _ballPresenter.OnBallReset.Subscribe(_ => _swipeManager.ResetSwipeTracking()).AddTo(_disposables);
            _timerManager.Timer.ObserveEveryValueChanged(t => t.Value).Subscribe(OnTimerChanged).AddTo(_disposables);
        }

        private void OnTimerChanged(float timer)
       {
            _timerUIPresenter.SetTimerValue(timer);

            if (timer <= 0)
            {
                _stateManager.ChangeState(GameStatesEnum.Reward);
            }
        }
    }
}