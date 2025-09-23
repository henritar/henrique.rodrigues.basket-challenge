using UnityEngine;

namespace Assets.Scripts.Runtime.Shared.Interfaces.Data
{
    public interface IShootingPositionData
    {
        Vector3[] PlayerShootingPositions { get; }
        Vector3[] NPCShootingPositions { get; }
    }
}