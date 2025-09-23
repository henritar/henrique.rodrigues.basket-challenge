using Assets.Scripts.Runtime.Camera;
using Assets.Scripts.Runtime.Factories;
using Assets.Scripts.Runtime.Gameplay.Ball;
using Assets.Scripts.Runtime.Gameplay.Interactables;
using Assets.Scripts.Runtime.Gameplay.Player;
using Assets.Scripts.Runtime.InputSystem.Gameplay;
using Assets.Scripts.Runtime.Managers;
using Assets.Scripts.Runtime.Managers.States.MainGame;
using Assets.Scripts.Runtime.ScriptableObjects;
using Assets.Scripts.Runtime.ScriptableObjects.InputSystem;
using Assets.Scripts.Runtime.Shared.Constants;
using Assets.Scripts.Runtime.Shared.EventBus;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using Assets.Scripts.Runtime.Shared.Interfaces.Factories.Player;
using Assets.Scripts.Runtime.Shared.Interfaces.InputSystem;
using Assets.Scripts.Runtime.Shared.Interfaces.InputSystem.Gameplay;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using Assets.Scripts.Runtime.Shared.Interfaces.StateMachine;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using Assets.Scripts.Runtime.UI.BackboardBonusUI;
using Assets.Scripts.Runtime.UI.DificultyMenu;
using Assets.Scripts.Runtime.UI.GameplayUI;
using Assets.Scripts.Runtime.UI.MainMenu;
using Assets.Scripts.Runtime.UI.RewardMenu;
using Assets.Scripts.Runtime.UI.Timer.TimerMenu;
using Assets.Scripts.Runtime.UI.Timer.TimerUI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts.Runtime.Bootstrap
{
    public class GameBootstrapVContainer : LifetimeScope
    {
        [SerializeField] private SO_GameplayInputReader _gameplayInputReader;
        [SerializeField] private SO_ShootingPositionData _shootingPositionData;
        [SerializeField] private SO_BackboardBonusData _backboardBonusData;
        [SerializeField] private SO_TimerData _timerData;
        [SerializeField] private SO_ShotResultData _shotResultData;
        [SerializeField] private SO_FireballData _fireballData;
        [SerializeField] private SO_NpcConfigData _npcConfigData;
        [SerializeField] private PlayerCreationalData[] _playerCreationDataArray;

        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        protected override void Configure(IContainerBuilder builder)
        {
            // Scriptable Objects
                _gameplayInputReader ??= ScriptableObject.CreateInstance<SO_GameplayInputReader>();
                _shootingPositionData ??= ScriptableObject.CreateInstance<SO_ShootingPositionData>();
                _backboardBonusData ??= ScriptableObject.CreateInstance<SO_BackboardBonusData>();
                _timerData ??= ScriptableObject.CreateInstance<SO_TimerData>();
                _shotResultData  ??= ScriptableObject.CreateInstance<SO_ShotResultData>();
                _fireballData ??= ScriptableObject.CreateInstance<SO_FireballData>();
                _npcConfigData ??= ScriptableObject.CreateInstance<SO_NpcConfigData>();

                builder.RegisterInstance(_gameplayInputReader).As<IGameplayInputReader>();
                builder.RegisterInstance(_shootingPositionData).As<IShootingPositionData>();
                builder.RegisterInstance(_backboardBonusData).As<IBackboardBonusData>();
                builder.RegisterInstance(_timerData).As<ITimerData>();
                builder.RegisterInstance(_shotResultData).As<IShotResultData>();
                builder.RegisterInstance(_fireballData).As<IFireballData>();
                builder.RegisterInstance(_npcConfigData).As<INpcConfigData>();

                builder.RegisterInstance(_musicAudioSource).Keyed(GameConstants.VContainer_MusicAudioSourceKey);
                builder.RegisterInstance(_sfxAudioSource).Keyed(GameConstants.VContainer_SFXAudioSourceKey);

            // Camera
                builder.RegisterComponentInHierarchy<CameraController>().As<ICameraController>();

            // Creational Data
                builder.RegisterInstance(_playerCreationDataArray);

            // Managers
                builder.Register<GameManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<GoalManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<ShotManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<SwipeManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<TimerManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<ShootingPositionManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<BackboardBonusManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<GameplayInputManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<NpcManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<SoundManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<GameStatesManager>(Lifetime.Singleton).As<IGameStateManager>();

            // Factories
                builder.Register<PlayerFactory>(Lifetime.Singleton).As<IPlayerFactory>();

            // Event Bus
                builder.Register<EventBus>(Lifetime.Singleton).As<IEventBus>();

            // Game States
                builder.Register<NoneGameState>(Lifetime.Singleton).As<IGameState>();
                builder.Register<MainMenuGameState>(Lifetime.Singleton).As<IGameState>();
                builder.Register<PlayingGameState>(Lifetime.Singleton).As<IGameState>();
                builder.Register<RewardGameState>(Lifetime.Singleton).As<IGameState>();

            //Interactables
                builder.RegisterComponentInHierarchy<BasketColliderDetector>().As<IBasketDetector>();
                builder.RegisterComponentInHierarchy<BasketPoint>().As<IBasketPoint>();
                builder.RegisterComponentInHierarchy<BackboardPoint>().As<IBackboardPoint>();
                builder.RegisterComponentInHierarchy<BackboardColliderDetector>().As<IBackboardColliderDetector>();

            // UI
                builder.RegisterComponentInHierarchy<MainMenuView>().As<IMainMenuView>();
                builder.Register<MainMenuModel>(Lifetime.Singleton).As<IMainMenuModel>();
                builder.Register<MainMenuPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.RegisterComponentInHierarchy<RewardMenuView>().As<IRewardMenuView>();
                builder.Register<RewardMenuModel>(Lifetime.Singleton).As<IRewardMenuModel>();
                builder.Register<RewardMenuPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.RegisterComponentInHierarchy<GameplayUIView>().As<IGameplayUIView>();
                builder.Register<GameplayUIModel>(Lifetime.Singleton).As<IGameplayUIModel>();
                builder.Register<GameplayUIPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.RegisterComponentInHierarchy<BackboardUIBonusView>().As<IBackboardBonusUIView>();
                builder.Register<BackboardBonusUIModel>(Lifetime.Singleton).As<IBackboardBonusUIModel>();
                builder.Register<BackboardBonusUIPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.RegisterComponentInHierarchy<TimerMenuView>().As<ITimerMenuView>();
                builder.Register<TimerMenuModel>(Lifetime.Singleton).As<ITimerMenuModel>();
                builder.Register<TimerMenuPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.RegisterComponentInHierarchy<TimerUIView>().As<ITimerUIView>();
                builder.Register<TimerUIModel>(Lifetime.Singleton).As<ITimerUIModel>();
                builder.Register<TimerUIPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.RegisterComponentInHierarchy<DifficultyMenuView>().As<IDifficultyMenuView>();
                builder.Register<DifficultyMenuModel>(Lifetime.Singleton).As<IDifficultyMenuModel>();
                builder.Register<DifficultyMenuPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.RegisterComponentInHierarchy<InputBarController>().As<IInputBarController>();
                builder.RegisterComponentInHierarchy<FireballBarController>().As<IFireballBarController>();

            // Input Handlers
                builder.Register<Gameplay_PlayingInputHandler>(Lifetime.Singleton).As<IPlayingInputHandler>();

            // Entry Point
                builder.Register<GameEntryPoint>(Lifetime.Singleton);
                builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
