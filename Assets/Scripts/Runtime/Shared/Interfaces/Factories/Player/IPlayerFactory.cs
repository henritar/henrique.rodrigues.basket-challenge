using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;

namespace Assets.Scripts.Runtime.Shared.Interfaces.Factories.Player
{
    public interface IPlayerFactory : IFactory<IPlayerPresenter, PlayerTypeEnum>
    {
    }
}