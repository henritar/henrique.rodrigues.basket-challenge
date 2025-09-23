using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Constants;
using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.Managers
{
    public class GoalManager : BaseManager, IGoalManager
    {
        private readonly IFireballData _fireballData;
        private readonly IFireballBarController _fireballBarController;
        private readonly IEventBus _eventBus;
        private CompositeDisposable _disposables;
        private ReactiveProperty<int> _currentScore;
        private ReactiveProperty<int> _fireballStreak;
        private bool _goal;

        private BonusTypeEnum _currentBonus = BonusTypeEnum.None;
        private ShotResultEnum _shotResult = ShotResultEnum.MissWeak;

        public int CurrentScore => _currentScore.Value;
        public int FireballThreshold => _fireballData.FireballThreshold;
        public int FireballStreak => _fireballStreak.Value;

        private CompositeDisposable _ballDisposable;

        public GoalManager(IFireballData fireballData, IFireballBarController fireballBarController, IEventBus eventBus) 
        {
            _fireballData = fireballData;
            _fireballBarController = fireballBarController;
            _eventBus = eventBus;
        }

        public override void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("GoalManager is already initialized. Skipping initialization.");
                return;
            }

            _disposables = new();
            _currentScore = new();
            _fireballStreak = new();

            _eventBus.OnEvent<GoalEvent>().Subscribe(OnGoalScored)
                .AddTo(_disposables);
            _eventBus.OnEvent<ShotEvent>().Subscribe(OnShotMade).AddTo(_disposables);
            _eventBus.OnEvent<UpdateBonusEvent>().Subscribe(OnNewBonus).AddTo(_disposables);
            _eventBus.OnEvent<GameStartEvent>().Subscribe(OnGameStart).AddTo(_disposables);

            _currentScore.Subscribe(OnUpdateScore).AddTo(_disposables);
            _fireballStreak.Subscribe(OnUpdateFireballStreak).AddTo(_disposables);

            _isInitialized = true;
        }

        public void ShowFireballBar(bool show)
        {
            _fireballBarController.EnableBarController(show);
        }

        private void OnGoalScored(GoalEvent goalEvent)
        {
            if (goalEvent.PlayerType == PlayerTypeEnum.NPC)
            {
                return;
            }

            _goal = true;
            int points = 0;
            if (_currentBonus != BonusTypeEnum.None && _shotResult == ShotResultEnum.BackboardBasket)
            {
                points = (int)_currentBonus;
            }
            else
            {
                points = _shotResult switch
                {
                    ShotResultEnum.PerfectShot => 3,
                    ShotResultEnum.RingTouch or ShotResultEnum.BackboardBasket => 2,
                    _ => 0
                };
            }

            bool shouldDoubleScore = _fireballStreak.Value >= GameConstants.FireballStreakThreshold;
            points *= shouldDoubleScore ? 2 : 1;
            _currentScore.Value += points;

            _fireballStreak.Value += points > 0 ? 1 : 0;

            Debug.Log($"Goal scored! Points: {points}");
        }

        private void OnGameStart(GameStartEvent gameStartEvent)
        {
            _currentScore.Value = 0;
            _fireballStreak.Value = 0;
            _goal = false;
        }

        private void OnUpdateScore(int newScore)
        {
            _eventBus.Publish(new UpdateScoreEvent(newScore));
        }

        private void OnUpdateFireballStreak(int newFireballStreak)
        {
            _fireballBarController.StacksFiller(newFireballStreak, FireballThreshold);
        }

        private void OnShotMade(ShotEvent shotEvent)
        {
            if (shotEvent.BallPresenter.BallPlayerType == PlayerTypeEnum.NPC)
            {
                return;
            }

            _shotResult = shotEvent.ShotResult;

            _ballDisposable = new();
            shotEvent.BallPresenter.OnBallReset.Subscribe(OnBallReset).AddTo(_ballDisposable);

            void OnBallReset(Unit unit)
            {
                _ballDisposable.Dispose();
                if (!_goal && (_shotResult == ShotResultEnum.MissWeak ||
                                _shotResult == ShotResultEnum.MissStrong ||
                                _shotResult == ShotResultEnum.RingTouch))
                {
                    _fireballStreak.Value = 0;
                }
                _goal = false;
            }
        }

        private void OnNewBonus(UpdateBonusEvent updateBonusEvent)
        {
            _currentBonus = updateBonusEvent.Bonus;
        }

        protected override void OnDestroying()
        {
            if (!_isInitialized)
            {
                return;
            }

            _disposables?.Dispose();
            _disposables = null;
            _isInitialized = false;
        }
    }
}