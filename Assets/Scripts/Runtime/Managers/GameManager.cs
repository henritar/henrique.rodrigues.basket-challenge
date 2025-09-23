using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.Factories.Player;
using Assets.Scripts.Runtime.Shared.Interfaces.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Runtime.Managers
{
    public class GameManager : BaseManager, IGameManager
    {
        private readonly IGameStateManager _gameStatesManager;
        private readonly ICameraController _cameraController;
        private readonly IPlayerFactory _playerFactory;

        public GameManager(IGameStateManager gameStateManager, ICameraController cameraController, IPlayerFactory playerFactory)
        {
            _gameStatesManager = gameStateManager;
            _cameraController = cameraController;
            _playerFactory = playerFactory;
        }

        public override void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("GameManager is already initialized. Skipping initialization.");
                return;
            }

            Debug.Log("GameManager initialized.");

            InitializeGame();
            _isInitialized = true;
        }

        private void InitializeGame()
        {
            var player = _playerFactory.Create(PlayerTypeEnum.Player);
            _cameraController.SetCameraFollowTarget(player.GetBall().BallTransform);
            _gameStatesManager.ChangeState(GameStatesEnum.MainMenu);
        }

        protected override void OnUpdate()
        {
            _gameStatesManager.Update();
        }

        protected override void OnFixedUpdate()
        {
            _gameStatesManager.FixedUpdate();
        }
    }
}