using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UniRx;

namespace Assets.Scripts.Runtime.UI.GameplayUI
{
    public class GameplayUIPresenter : BasePresenter<IGameplayUIModel, IGameplayUIView>, IGameplayUIPresenter
    {
        private readonly IEventBus _eventBus;
        private CompositeDisposable _disposables = new CompositeDisposable();

        public GameplayUIPresenter(IGameplayUIModel model, IGameplayUIView view, IEventBus eventBus) : base(model, view)
        {
            _eventBus = eventBus;
        }

        public void ShowUI(bool show)
        {
            Model.SetUIVisible(show);
        }

        protected override void OnInitialize()
        {
            _disposables = new CompositeDisposable();
            
        }

        protected override void SubscribeToEvents()
        {
            _eventBus.OnEvent<UpdateScoreEvent>()
                .Subscribe(OnUpdateScore)
                .AddTo(_disposables);


            Model.IsUIVisible.Subscribe(OnUIVisibleChanged).AddTo(_disposables);

            Model.PlayerCurrentPoints
                .Subscribe(points => View.UpdatePlayerScore(points))
                .AddTo(_disposables);

            Model.NpcCurrentPoints
                .Subscribe(points => View.UpdateNpcScore(points))
                .AddTo(_disposables);
        }

        protected override void UnsubscribeFromEvents()
        {
        }

        private void OnUpdateScore(UpdateScoreEvent scoreEvent)
        {
            switch (scoreEvent.PlayerType)
            {
                case Enums.PlayerTypeEnum.Player:
                    Model.UpdatePlayerPoints(scoreEvent.Points);
                    break;
                case Enums.PlayerTypeEnum.NPC:
                default:
                    Model.UpdateNpcPoints(scoreEvent.Points);
                    break;
            }
        }

        private void OnUIVisibleChanged(bool visible)
        {
            switch (visible)
            {
                case true:
                    View.Show();
                    break;
                case false:
                    View.Hide();
                    break;
            }
        }

        protected override void Cleanup()
        {
            _disposables.Dispose();
            _disposables = null;
        }
    }
}