namespace Assets.Scripts.Runtime.Shared.Interfaces
{
    public interface ISoundManager
    {
        public void PlaySound(string soundName);
        public void PlayMusic(string musicName, bool loop = true);
        public void StopMusic();
        public void SetSFXVolume(float volume);
        public void SetMusicVolume(float volume);
    }
}