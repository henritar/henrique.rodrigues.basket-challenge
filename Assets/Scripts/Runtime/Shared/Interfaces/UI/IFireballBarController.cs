using Assets.Scripts.Runtime.Shared.Interfaces.UI.BarController;

namespace Assets.Scripts.Runtime.Shared.Interfaces.UI
{
    public interface IFireballBarController : IBarController
    {
        void StacksFiller(int streak, int threshHold);
    }
}