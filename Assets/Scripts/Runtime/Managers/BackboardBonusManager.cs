using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.Managers
{
    public class BackboardBonusManager : BaseManager, IBackboardBonusManager
    {
        private readonly IEventBus _eventBus;
        private readonly IBackboardBonusData _backboardBonusData;

        private float _noBonusChance;
        private float _commonChance;
        private float _rareChance;
        private float _veryRareChance;

        private bool isGenerating;

        private CancellationTokenSource cancellationTokenSource;
        private CompositeDisposable _disposables;

        public BackboardBonusManager(IBackboardBonusData backboardBonusData, IEventBus eventBus)
        {
            _backboardBonusData = backboardBonusData;
            _eventBus = eventBus;
        }

        public override void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("SwipeManager is already initialized. Skipping initialization.");
                return;
            }

            NormalizeProbabilities();

            _disposables = new();

            _isInitialized = true;
        }

        public void StartBonusGeneration()
        {
            if (isGenerating) 
            { 
                return;
            }

            isGenerating = true;
            cancellationTokenSource = new CancellationTokenSource();

            BonusGenerationLoopAsync(cancellationTokenSource.Token).Forget();
        }

        public void StopBonusGeneration()
        {
            isGenerating = false;
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }

        private async UniTaskVoid BonusGenerationLoopAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await SpawnBonusAsync(cancellationToken);
            }
        }

        private async UniTask SpawnBonusAsync(CancellationToken cancellationToken)
        {
            var bonusType = RollBonusType();
            
            _eventBus.Publish(new UpdateBonusEvent(bonusType));

            float waitTime = UnityEngine.Random.Range(_backboardBonusData.MinBonusInterval, _backboardBonusData.MaxBonusInterval);
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: cancellationToken);
        }

        private BonusTypeEnum RollBonusType()
        {
            float roll = UnityEngine.Random.Range(0f, 1f);

            if (roll <= _noBonusChance)
            {
                return BonusTypeEnum.None;
            }
            else if (roll <= _noBonusChance + _commonChance)
            {
                return BonusTypeEnum.FourPoints;
            }
            else if (roll <= _noBonusChance + _commonChance + _rareChance)
            {
                return BonusTypeEnum.SixPoints;
            }
            else if (roll <= _noBonusChance + _commonChance + _rareChance + _veryRareChance)
            {
                return BonusTypeEnum.EightPoints;
            }
            else
                return BonusTypeEnum.None;
        }

        private void NormalizeProbabilities()
        {
            float no = _backboardBonusData.NoBonusChance;
            float common = _backboardBonusData.CommonBonusChance;
            float rare = _backboardBonusData.RareBonusChance;
            float veryRare = _backboardBonusData.VeryRareBonusChance;

            float total = no + common + rare + veryRare;

            _noBonusChance = no / total;
            _commonChance = common / total;
            _rareChance = rare / total;
            _veryRareChance = veryRare / total;
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