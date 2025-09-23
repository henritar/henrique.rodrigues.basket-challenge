using Assets.Scripts.Runtime.Shared.Interfaces.MVP;

namespace Assets.Scripts.Runtime.Shared.Interfaces.Factories
{
    public interface IFactory<TPresenter, TCreationData> where TPresenter : IBasePresenter
    {
        public TPresenter Create(TCreationData creationData);
    }
}