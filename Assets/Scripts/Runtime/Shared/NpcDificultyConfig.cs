using Assets.Scripts.Runtime.Enums;

namespace Assets.Scripts.Runtime.Shared
{
    public struct NpcDificultyConfig
    {
        public NpcDificultyEnum NpcDificultyEnum;
        public float PerfectShotChance;
        public float RingShotChance;
        public float BackboardShotChance;
        public float MissShotWeakChance;
        public float MissShotStrongChance;
    }
}