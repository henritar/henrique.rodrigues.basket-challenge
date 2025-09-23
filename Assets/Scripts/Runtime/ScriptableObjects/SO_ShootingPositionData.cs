using Assets.Scripts.Runtime.Shared.Constants;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using UnityEngine;

namespace Assets.Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Shooting Position Data", menuName = "Scriptable Objects/Data/ShootingPositionData")]
    public class SO_ShootingPositionData : ScriptableObject, IShootingPositionData
    {
        [SerializeField] private Vector3[] _playerShootingPositionArray = new Vector3[] {new Vector3(0,0,GameConstants.MinFreeThrowLineDistance) };
        [SerializeField] private Vector3[] _npcShootingPositionArray = new Vector3[] {new Vector3(0,0,GameConstants.MinFreeThrowLineDistance) };

        public Vector3[] PlayerShootingPositions => _playerShootingPositionArray;
        public Vector3[] NPCShootingPositions => _npcShootingPositionArray;
    }
}