using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using UnityEngine;

namespace Assets.Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New FireballData", menuName = "Scriptable Objects/Data/FireballData", order = 6)]
    public class SO_FireballData : ScriptableObject, IFireballData
    {
        [Tooltip("Min value is 1; Max value is 10")][SerializeField] private int _fireballThreshold = 3;

        public int FireballThreshold => Mathf.Clamp(_fireballThreshold, 1, 10);
    }
}