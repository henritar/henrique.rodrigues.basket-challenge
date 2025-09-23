using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Gameplay.Ball;
using Assets.Scripts.Runtime.Gameplay.Player;
using Assets.Scripts.Runtime.Shared.Interfaces.Factories.Player;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Runtime.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly PlayerCreationalData[] _creationalDatas;
        private Dictionary<PlayerTypeEnum, IPlayerPresenter> _presenters = new();

        public PlayerFactory(PlayerCreationalData[] creationalDatas)
        {
            _creationalDatas = creationalDatas;
        }

        public IPlayerPresenter Create(PlayerTypeEnum playerType)
        {
            if (_presenters.ContainsKey(playerType)) 
            {
                return _presenters[playerType];
            }

            PlayerCreationalData playerData = default;
            foreach (var data in _creationalDatas)
            {
                if (data.playerType == playerType)
                {
                    playerData = data;
                }
            }

            if (playerData.playerPrefab == null)
            {
                throw new ArgumentNullException($"No PlayerCreationalData for player type: {playerType}");
            }

            GameObject playerInstance = UnityEngine.Object.Instantiate(
                playerData.playerPrefab);

            var playerView = playerInstance.GetComponent<IPlayerView>();
            var playerBallView = playerInstance.GetComponentInChildren<IBallView>();

            IBallModel playerBallModel = new BallModel(playerType);
            IBallPresenter playerBallPresenter = new BallPresenter(playerBallModel, playerBallView);

            IPlayerModel playerModel = new PlayerModel(playerBallPresenter);
            IPlayerPresenter playerPresenter = new PlayerPresenter(playerModel, playerView);

            _presenters.TryAdd(playerType, playerPresenter);

            return playerPresenter;
        }
    }
}