using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Constants;
using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.Managers
{
    public class SwipeManager : BaseManager, ISwipeManager
    {
        private readonly IGameplayInputManager _inputManager;
        private readonly IEventBus _eventBus;
        private readonly IInputBarController _inputBarController;
        private readonly IShotResultData _resultData;

        private IBallPresenter _ballPresenter;
        private CompositeDisposable _disposables;

        private Vector2 _startPosition;
        private Vector2 _currentPosition;
        private float _startTime;
        private bool _isTracking;
        private bool _hasCalculated;
        private CancellationTokenSource _swipeCts;

        public SwipeManager(IGameplayInputManager inputManager, IShotResultData resultData,
            IInputBarController inputBarController, IEventBus eventBus)
        {
            _inputManager = inputManager;
            _resultData = resultData;
            _inputBarController = inputBarController;
            _eventBus = eventBus;
        }

        public override void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("SwipeManager is already initialized. Skipping initialization.");
                return;
            }

            _disposables = new();

            _inputBarController.SetZonePosition(_resultData);

            _isInitialized = true;
        }

        public void ShowInputBar(bool show)
        {
            _inputBarController.EnableBarController(show);
        }

        public void StartSwipeTracking(IBallPresenter ballPresenter)
        {
            if (_isTracking)
            {
                return;
            }

            _ballPresenter = ballPresenter;
            _startPosition = _inputManager.PointerPosition;
            _currentPosition = _startPosition;
            _startTime = Time.time;
            _isTracking = true;

            _swipeCts?.Cancel();
            _swipeCts = new CancellationTokenSource();

            TrackSwipeAsync(_swipeCts.Token).Forget();
        }

        private async UniTask TrackSwipeAsync(CancellationToken ct)
        {
            Vector2 lastPosition = _startPosition;
            float lastMoveTime = _startTime;
            float maxPowerReached = 0f;

            while (_isTracking && !ct.IsCancellationRequested)
            {
                await UniTask.NextFrame(ct);

                _currentPosition = _inputManager.PointerPosition;
                Vector2 deltaMove = _currentPosition - lastPosition;
                float currentTime = Time.time;
                float deltaTime = currentTime - lastMoveTime;

                // Verificar se deve parar
                if (ShouldStopTracking(deltaMove, deltaTime, currentTime))
                {
                    CalculateAndReturnPower();
                    break;
                }

                Vector2 totalDelta = _currentPosition - _startPosition;
                float upwardDistance = Mathf.Max(0, totalDelta.y);
                float currentPower = Mathf.Clamp01(upwardDistance / GameConstants.MaxSwipeDistance) * 100f;

                maxPowerReached = Mathf.Max(maxPowerReached, currentPower);
                _inputBarController.SetPower(maxPowerReached);

                lastPosition = _currentPosition;
                lastMoveTime = currentTime;
            }
        }

        private bool ShouldStopTracking(Vector2 deltaMove, float deltaTime, float currentTime)
        {
            float currentSpeed = deltaMove.magnitude / deltaTime;
            bool isMovingUp = deltaMove.y > 0;
            bool isMovingFastEnough = currentSpeed >= GameConstants.MinSwipeSpeed;
            bool withinTimeWindow = (currentTime - _startTime) <= GameConstants.SwipeTimeWindow;

            return !withinTimeWindow ||
                   (!isMovingUp && deltaMove.magnitude > 5f) ||
                   (deltaTime > 0.1f && !isMovingFastEnough);
        }

        public void EndSwipeTracking()
        {
            if (_isTracking && !_hasCalculated)
            {
                CalculateAndReturnPower();
            }
        }

        private void CalculateAndReturnPower()
        {
            _hasCalculated = true;
            _swipeCts?.Cancel();

            Vector2 totalDelta = _currentPosition - _startPosition;
            float upwardDistance = Mathf.Max(0, totalDelta.y);

            float power = Mathf.Clamp01(upwardDistance / GameConstants.MaxSwipeDistance) * 100f;

            ShotResultEnum shotResult = SelectShotResult(power);
            Debug.Log($"{shotResult.ToString()} with {power} power");
            _eventBus.Publish(new ShotEvent(_ballPresenter, shotResult));
        }

        private ShotResultEnum SelectShotResult(float power)
        {
            var powerPercent = power / 100;

            foreach (var range in _resultData.ShotResultRanges)
            {
                if (powerPercent >= range.MinPower && powerPercent <= range.MaxPower)
                {
                    return range.Result;
                }
            }
 
            return ShotResultEnum.MissWeak;
        }

        public void ResetSwipeTracking()
        {
            _isTracking = false;
            _hasCalculated = false;
            _inputBarController.SetPower(0f);
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