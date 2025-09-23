using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared.Interfaces;

namespace Assets.Scripts.Runtime.Shared.EventBus.Events
{
    public class GoalEvent : IGameEvent
    {
        private PlayerTypeEnum _playerType;
        public PlayerTypeEnum PlayerType { get; }
        public GoalEvent(PlayerTypeEnum playerType) 
        { 
            _playerType = playerType;
        }
    }
}