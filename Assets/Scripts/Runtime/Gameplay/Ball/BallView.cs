using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using UnityEngine;

namespace Assets.Scripts.Runtime.Gameplay.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallView : BaseView, IBallView
    {

        private Rigidbody _rigidbody;
        private PlayerTypeEnum _playerType;
        public Rigidbody Rigidbody => _rigidbody;
        public PlayerTypeEnum BallPlayerType => _playerType;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetPlayerType(PlayerTypeEnum playerType)
        {
            _playerType = playerType;
        }
    }
}