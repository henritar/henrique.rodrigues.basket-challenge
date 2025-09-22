using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using Assets.Scripts.Runtime.Shared.Interfaces.UI.BarController;

namespace Assets.Scripts.Runtime.Shared.Interfaces.UI
{
    public interface IInputBarController : IBarController
    {
        void SetPower(float powerPercent);
        void SetZonePosition(IShotResultData shotData);
    }
}