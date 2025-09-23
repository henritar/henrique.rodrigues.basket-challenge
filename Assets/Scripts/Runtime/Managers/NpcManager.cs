using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using Assets.Scripts.Runtime.Shared.Interfaces.Factories.Player;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.Managers
{
    public class NpcManager : BaseManager, INpcManager
    {
        private readonly INpcConfigData _npcConfigData;
        private readonly IPlayerFactory _playerFactory;
        private readonly IEventBus _eventBus;

        private IPlayerPresenter _npcPresenter;

        private CompositeDisposable _disposables;
        private CancellationTokenSource _cts;

        private float _perfectShotChance;
        private float _backboardShotChance;
        private float _ringShotChance;
        private float _missShotStrongChance;
        private float _missShotWeakChance;

        public NpcManager(INpcConfigData npcConfigData, IPlayerFactory playerFactory, IEventBus eventBus)
        {
            _npcConfigData = npcConfigData;
            _playerFactory = playerFactory;
            _eventBus = eventBus;
        }

        public override void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("NPCManager is already initialized. Skipping initialization.");
                return;
            }

            _disposables = new();

            NormalizeShotProbabilities();

            _eventBus.OnEvent<GameStartEvent>().Subscribe(StartNPC)
            .AddTo(_disposables);
            _eventBus.OnEvent<TimerEndedEvent>().Subscribe(StopNPC)
            .AddTo(_disposables);

            _npcPresenter = _playerFactory.Create(Enums.PlayerTypeEnum.NPC);

            _isInitialized = true;
        }

        private void StartNPC(GameStartEvent gameStartEvent)
        {
            _cts = new CancellationTokenSource();
            NPCShotBehaviour(_cts.Token).Forget();
        }

        private void StopNPC(TimerEndedEvent timerEndedEvent)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTaskVoid NPCShotBehaviour(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    var shotResult = RollShotResult();
                    Debug.Log($"Npc rolled {shotResult.ToString()}");
                    _eventBus.Publish(new ShotEvent(_npcPresenter.GetBall(), shotResult));

                    await UniTask.Delay(
                    TimeSpan.FromSeconds(_npcConfigData.ShotInterval),
                    cancellationToken: ct
                    );
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        private Enums.ShotResultEnum RollShotResult()
        {
            float roll = UnityEngine.Random.Range(0f, 1f);

            if (roll <= _perfectShotChance)
            {
                return Enums.ShotResultEnum.PerfectShot;
            }
            else if (roll <= _perfectShotChance + _backboardShotChance)
            {
                return Enums.ShotResultEnum.BackboardBasket;
            }
            else if (roll <= _perfectShotChance + _backboardShotChance + _ringShotChance)
            {
                return Enums.ShotResultEnum.RingTouch;
            }
            else if (roll <= _perfectShotChance + _backboardShotChance + _ringShotChance + _missShotStrongChance)
            {
                return Enums.ShotResultEnum.MissStrong;
            }
            else
            {
                return Enums.ShotResultEnum.MissWeak;
            }
        }

        private void NormalizeShotProbabilities()
        {
            var difficultyConfig = _npcConfigData.NpcDificultyConfigs[0];

            float perfect = difficultyConfig.PerfectShotChance;
            float backboard = difficultyConfig.BackboardShotChance;
            float ring = difficultyConfig.RingShotChance;
            float missStrong = difficultyConfig.MissShotStrongChance;
            float missWeak = difficultyConfig.MissShotWeakChance;

            float total = perfect + backboard + ring + missStrong + missWeak;

            _perfectShotChance = perfect / total;
            _backboardShotChance = backboard / total;
            _ringShotChance = ring / total;
            _missShotStrongChance = missStrong / total;
            _missShotWeakChance = missWeak / total;
        }

        protected override void OnDestroying()
        {
            if (!_isInitialized)
            {
                return;
            }

            _disposables?.Dispose();
            _cts?.Cancel();
            _cts?.Dispose();
            _isInitialized = false;
        }
    }
}