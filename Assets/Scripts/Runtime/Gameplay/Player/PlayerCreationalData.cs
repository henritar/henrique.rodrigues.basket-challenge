using Assets.Scripts.Runtime.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.Gameplay.Player
{
    [Serializable]
    public struct PlayerCreationalData
    {
        public GameObject playerPrefab;
        public PlayerTypeEnum playerType;
    }
}