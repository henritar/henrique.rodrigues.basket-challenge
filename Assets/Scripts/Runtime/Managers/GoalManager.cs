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
        private ReactiveProperty<int> _playerCurrentScore;
        private ReactiveProperty<int> _npcCurrentScore;
        private ReactiveProperty<int> _fireballStreak;
        private bool _goal;

        private BonusTypeEnum _currentBonus = BonusTypeEnum.None;
        private ShotResultEnum _playerShotResult = ShotResultEnum.MissWeak;
        private ShotResultEnum _npcShotResult = ShotResultEnum.MissWeak;

        public int PlayerCurrentScore => _playerCurrentScore.Value;
        public int NpcCurrentScore => _npcCurrentScore.Value;
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
            _playerCurrentScore = new();
            _npcCurrentScore = new();
            _fireballStreak = new();

            _eventBus.OnEvent<GoalEvent>().Subscribe(OnGoalScored)
                .AddTo(_disposables);
            _eventBus.OnEvent<ShotEvent>().Subscribe(OnShotMade).AddTo(_disposables);
            _eventBus.OnEvent<UpdateBonusEvent>().Subscribe(OnNewBonus).AddTo(_disposables);
            _eventBus.OnEvent<GameStartEvent>().Subscribe(OnGameStart).AddTo(_disposables);

            _playerCurrentScore.Subscribe(points => OnUpdateScore(points, PlayerTypeEnum.Player)).AddTo(_disposables);
            _npcCurrentScore.Subscribe(points => OnUpdateScore(points, PlayerTypeEnum.NPC)).AddTo(_disposables);
            _fireballStreak.Subscribe(OnUpdateFireballStreak).AddTo(_disposables);

            _isInitialized = true;
        }

        public void ShowFireballBar(bool show)
        {
            _fireballBarController.EnableBarController(show);
        }

        private void OnGoalScored(GoalEvent goalEvent)
        {
            int points = 0;

            var shotResult = goalEvent.PlayerType == PlayerTypeEnum.Player ? _playerShotResult : _npcShotResult;

            if (_currentBonus != BonusTypeEnum.None && shotResult == ShotResultEnum.BackboardBasket)
            {
                points = (int)_currentBonus;
            }
            else
            {
                points = shotResult switch
                {
                    ShotResultEnum.PerfectShot => 3,
                    ShotResultEnum.RingTouch or ShotResultEnum.BackboardBasket => 2,
                    _ => 0
                };
            }

            
            if (goalEvent.PlayerType == PlayerTypeEnum.Player)
            {
                _goal = true;
                bool shouldDoubleScore = _fireballStreak.Value >= GameConstants.FireballStreakThreshold;
                points *= shouldDoubleScore ? 2 : 1;
                _playerCurrentScore.Value += points;
                _fireballStreak.Value += points > 0 ? 1 : 0;
            }
            else
            {
                _npcCurrentScore.Value += points;
            }
        }

        private void OnGameStart(GameStartEvent gameStartEvent)
        {
            _playerCurrentScore.Value = 0;
            _npcCurrentScore.Value = 0;
            _fireballStreak.Value = 0;
            _goal = false;
        }

        private void OnUpdateScore(int newScore, PlayerTypeEnum playerType)
        {
            _eventBus.Publish(new UpdateScoreEvent(newScore, playerType));
        }

        private void OnUpdateFireballStreak(int newFireballStreak)
        {
            _fireballBarController.StacksFiller(newFireballStreak, FireballThreshold);
        }

        private void OnShotMade(ShotEvent shotEvent)
        {
            if (shotEvent.BallPresenter.BallPlayerType == PlayerTypeEnum.NPC)
            {
                _npcShotResult = shotEvent.ShotResult;
                return;
            }

            _playerShotResult = shotEvent.ShotResult;

            _ballDisposable = new();
            shotEvent.BallPresenter.OnBallReset.Subscribe(OnBallReset).AddTo(_ballDisposable);

            void OnBallReset(Unit unit)
            {
                _ballDisposable.Dispose();
                if (!_goal && (_playerShotResult == ShotResultEnum.MissWeak ||
                                _playerShotResult == ShotResultEnum.MissStrong ||
                                _playerShotResult == ShotResultEnum.RingTouch))
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