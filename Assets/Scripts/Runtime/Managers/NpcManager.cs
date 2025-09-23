using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
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
        private readonly IEventBus _eventBus;
        private readonly IPlayerFactory _playerFactory;

        private IPlayerPresenter _npcPresenter; 

        private CompositeDisposable _disposables;
        private CancellationTokenSource _cts;

        private const float ShotIntervalSeconds = 3f;
        public NpcManager(IPlayerFactory playerFactory, IEventBus eventBus)
        {
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
                    _eventBus.Publish(new ShotEvent(_npcPresenter.GetBall(), Enums.ShotResultEnum.PerfectShot));

                    await UniTask.Delay(
                        System.TimeSpan.FromSeconds(ShotIntervalSeconds),
                        cancellationToken: ct
                    );
                }
            }
            catch (OperationCanceledException)
            {
                
            }
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