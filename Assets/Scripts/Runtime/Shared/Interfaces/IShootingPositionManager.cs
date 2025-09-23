using Assets.Scripts.Runtime.Enums;

namespace Assets.Scripts.Runtime.Shared.Interfaces
{
    public interface IShootingPositionManager : IBaseManager
    {
        void MoveToRandomShootingPosition(PlayerTypeEnum playerType);
    }
}