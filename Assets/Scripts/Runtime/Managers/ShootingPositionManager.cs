using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using Assets.Scripts.Runtime.Shared.Interfaces.Factories.Player;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace Assets.Scripts.Runtime.Managers
{
    public class ShootingPositionManager : BaseManager, IShootingPositionManager
    {
        private Dictionary<PlayerTypeEnum, IPlayerPresenter> _playerPresenters;

        private readonly IPlayerFactory _playerFactory;
        private readonly IShootingPositionData _shootingData;
        private readonly IEventBus _eventBus;

        private CompositeDisposable _disposables;

        public ShootingPositionManager(IShootingPositionData shootingData, IPlayerFactory playerFactory, IEventBus eventBus)
        {
            _shootingData = shootingData;
            _playerFactory = playerFactory;
            _eventBus = eventBus;
        }

        public override void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("ShootingPositionManager is already initialized. Skipping initialization.");
                return;
            }

            _disposables = new();
            _playerPresenters = new();

            foreach (var playerType in Enum.GetValues(typeof(PlayerTypeEnum))) 
            {
                var pType = (PlayerTypeEnum)playerType;
                var player = _playerFactory.Create(pType);
                _playerPresenters.TryAdd(pType, player);
            }

            _eventBus.OnEvent<GoalEvent>().Subscribe(DelayedMoveToPosition).AddTo(_disposables);

            _isInitialized = true;
        }

        public void MoveToRandomShootingPosition(PlayerTypeEnum playerType)
        {
            var player = _playerPresenters[playerType];

            if (_shootingData.ShootingPositions == null || _shootingData.ShootingPositions.Length == 0)
            {
                Debug.LogWarning("ShootingPositionManager: No shooting positions available.");
                return;
            }
            int randomIndex = UnityEngine.Random.Range(0, _shootingData.ShootingPositions.Length);
            player.GetBall().ResetBall();
            player.MoveToPosition(_shootingData.ShootingPositions[randomIndex]);
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

        private void DelayedMoveToPosition(GoalEvent goalEvent)
        {
            UniTask.Delay(TimeSpan.FromSeconds(0.15f)).ContinueWith(() =>
            {
                MoveToRandomShootingPosition(goalEvent.PlayerType);
            }).Forget();
        }
    }
}