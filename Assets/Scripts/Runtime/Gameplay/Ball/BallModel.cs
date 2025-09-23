using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.Gameplay.Ball
{
    public class BallModel : BaseModel, IBallModel
    {
        private readonly PlayerTypeEnum playerType;
        private ReactiveProperty<Vector3> _startPosition = new();

        public IReadOnlyReactiveProperty<Vector3> StartPosition => _startPosition;

        public PlayerTypeEnum PlayerType => playerType;

        public BallModel(PlayerTypeEnum playerType)
        {
            this.playerType = playerType;
        }

        public void SetStartPosition(Vector3 pos)
        {
            _startPosition.Value = pos;
        }
    }
}