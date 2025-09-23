using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New ShotResultData", menuName = "Scriptable Objects/Data/ShotResultData", order = 1)]
    public class SO_ShotResultData : ScriptableObject, IShotResultData
    {
        [SerializeField] private ShotResultRange[] _shotResultRanges = new ShotResultRange[] 
        {
            new ShotResultRange { MinPower = 0.0f, MaxPower = 0.50f, Result = ShotResultEnum.MissWeak },
            new ShotResultRange { MinPower = 0.50f, MaxPower = 0.60f, Result = ShotResultEnum.RingTouch },
            new ShotResultRange { MinPower = 0.60f, MaxPower = 0.75f, Result = ShotResultEnum.PerfectShot },
            new ShotResultRange { MinPower = 0.75f, MaxPower = 0.85f, Result = ShotResultEnum.RingTouch },
            new ShotResultRange { MinPower = 0.85f, MaxPower = 0.95f, Result = ShotResultEnum.BackboardBasket },
            new ShotResultRange { MinPower = 0.95f, MaxPower = 1f, Result = ShotResultEnum.MissStrong },
        };

    public ShotResultRange[] ShotResultRanges => _shotResultRanges;
    }

    [Serializable]
    public struct ShotResultRange
    {
        public float MinPower;
        public float MaxPower;
        public ShotResultEnum Result;

    }
}