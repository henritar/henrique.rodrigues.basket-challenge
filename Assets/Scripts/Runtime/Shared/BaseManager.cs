using Assets.Scripts.Runtime.Shared.Interfaces;
using System;
using VContainer.Unity;

namespace Assets.Scripts.Runtime.Shared
{
    public abstract class BaseManager : IBaseManager, IInitializable, IStartable, ITickable, IFixedTickable, IDisposable
    {
        protected bool _isInitialized = false;
        public bool IsInitialized => _isInitialized;

        public void Start()
        {
            OnStart();
        }

        public void Tick()
        {
            if (!_isInitialized)
            {
                return;
            }

            OnUpdate();
        }
        public void FixedTick()
        {
            if (!_isInitialized)
            {
                return;
            }

            OnFixedUpdate();
        }

        public void Dispose()
        {
            OnDestroying();
        }

        public abstract void Initialize();

        protected virtual void OnAwake()
        {

        }
        protected virtual void OnStart()
        {

        }
        protected virtual void OnUpdate()
        {

        }
        protected virtual void OnFixedUpdate()
        {

        }
        protected virtual void OnDestroying()
        {

        }
    }
}