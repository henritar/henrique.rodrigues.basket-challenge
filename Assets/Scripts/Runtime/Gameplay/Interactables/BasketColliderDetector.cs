using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
using Assets.Scripts.Runtime.Shared.Interfaces.Interactables;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Runtime.Gameplay.Interactables
{
    public class BasketColliderDetector : MonoBehaviour, IBasketDetector
    {
        [Inject]
        private readonly IEventBus _eventBus;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Basketball"))
            {
                var ballView = other.GetComponent<IBallView>();

                if (ballView == null)
                {
                    return;
                }

                _eventBus.Publish(new GoalEvent(ballView.BallPlayerType));
            }
        }
    }
}