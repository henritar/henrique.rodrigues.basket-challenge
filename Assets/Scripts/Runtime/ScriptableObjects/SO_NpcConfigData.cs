using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using UnityEngine;

namespace Assets.Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New NpcConfigData", menuName = "Scriptable Objects/Data/NpcConfigData", order = 5)]
    public class SO_NpcConfigData : ScriptableObject, INpcConfigData
    {
        [SerializeField] private NpcDifficultyConfig[] npcDificultyConfigs = new NpcDifficultyConfig[] 
        {
            new NpcDifficultyConfig { 
                NpcDifficultyEnum = Enums.NpcDifficultyEnum.easy,
                PerfectShotChance = 0.2f,
                BackboardShotChance = 0.2f,
                RingShotChance = 0.2f,
                MissShotStrongChance = 0.2f,
                MissShotWeakChance = 0.2f,
                ShotInterval = 3f } 
        };
        public NpcDifficultyConfig[] NpcDifficultyConfigs => npcDificultyConfigs;
    }
}