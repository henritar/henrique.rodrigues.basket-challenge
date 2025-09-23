using Assets.Scripts.Runtime.Enums;
using System;

namespace Assets.Scripts.Runtime.Shared
{
    [Serializable]
    public struct NpcDifficultyConfig
    {
        public NpcDifficultyEnum NpcDifficultyEnum;
        public float PerfectShotChance;
        public float RingShotChance;
        public float BackboardShotChance;
        public float MissShotWeakChance;
        public float MissShotStrongChance;
        public float ShotInterval;
    }
}