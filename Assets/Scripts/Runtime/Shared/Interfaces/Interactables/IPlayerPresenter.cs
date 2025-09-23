using Assets.Scripts.Runtime.Shared.Interfaces.MVP;
using UnityEngine;

namespace Assets.Scripts.Runtime.Shared.Interfaces.Interactables
{
    public interface IPlayerPresenter : IBasePresenter
    {
        void MoveToPosition(Vector3 xPosition);
        IBallPresenter GetBall();
    }
}