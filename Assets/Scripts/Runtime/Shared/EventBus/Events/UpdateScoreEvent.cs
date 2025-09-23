using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared.Interfaces;

namespace Assets.Scripts.Runtime.Shared.EventBus.Events
{
    public class UpdateScoreEvent : IGameEvent
    {
        public int Points { get; private set; }
        public PlayerTypeEnum PlayerType { get; private set; }

        public UpdateScoreEvent(int points, PlayerTypeEnum playerType)
        {
            Points = points;
            PlayerType = playerType;
        }
    }
}