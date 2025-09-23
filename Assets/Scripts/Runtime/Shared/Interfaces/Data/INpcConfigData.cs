namespace Assets.Scripts.Runtime.Shared.Interfaces.Data
{
    public interface INpcConfigData
    {
        public float ShotInterval { get; }
        public NpcDificultyConfig[] NpcDificultyConfigs { get; }
    }
}