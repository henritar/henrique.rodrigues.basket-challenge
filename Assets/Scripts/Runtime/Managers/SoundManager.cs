using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Constants;
using Assets.Scripts.Runtime.Shared.EventBus.Events;
using Assets.Scripts.Runtime.Shared.Interfaces;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Runtime.Managers
{
    public class SoundManager : BaseManager, ISoundManager
    {
        private readonly AudioSource _musicSource;
        private readonly AudioSource _sfxSource;
        private IEventBus _eventBus;
        private Dictionary<string, AudioClip> _soundLibrary = new Dictionary<string, AudioClip>();

        private CompositeDisposable _disposables;

        public SoundManager([Key(GameConstants.VContainer_MusicAudioSourceKey)] AudioSource musicSource,
            [Key(GameConstants.VContainer_SFXAudioSourceKey)] AudioSource sfxSource, IEventBus eventBus)
        {
            _musicSource = musicSource;
            _sfxSource = sfxSource;
            _eventBus = eventBus;
        }

        public override void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("SoundManager is already initialized. Skipping initialization.");
                return;
            }

            _disposables = new();

            LoadSounds();

            _eventBus.OnEvent<GoalEvent>().Subscribe(_ => PlaySound(GameConstants.NetSound)).AddTo(_disposables);
            _eventBus.OnEvent<TimerEndedEvent>().Subscribe(_ => PlaySound(GameConstants.BuzzerGameOverSound)).AddTo(_disposables);
            _eventBus.OnEvent<GameStartEvent>().Subscribe(_ => PlaySound(GameConstants.RefereeWhistleSound)).AddTo(_disposables);
            _eventBus.OnEvent<BackboardHitEvent>().Subscribe(_ => PlaySound(GameConstants.BackbordSound)).AddTo(_disposables);
        }

        private void LoadSounds()
        {
            AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/");
            foreach (var clip in clips)
            {
                _soundLibrary[clip.name] = clip;
            }
        }

        public void PlaySound(string soundName)
        {
            if (_soundLibrary.ContainsKey(soundName))
            {
                _sfxSource.PlayOneShot(_soundLibrary[soundName]);
            }
        }

        public void PlayMusic(string musicName, bool loop = true)
        {
            if (_soundLibrary.ContainsKey(musicName))
            {
                _musicSource.clip = _soundLibrary[musicName];
                _musicSource.loop = loop;
                _musicSource.Play();
            }
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }

        public void SetSFXVolume(float volume)
        {
            _sfxSource.volume = volume;
        }

        public void SetMusicVolume(float volume)
        {
            _musicSource.volume = volume;
        }

        protected override void OnStart()
        {
            PlayMusic(GameConstants.AmbientSound);
        }

        protected override void OnDestroying()
        {
            if (!_isInitialized)
            {
                return;
            }

            _disposables?.Dispose();
            _disposables = null;
            _isInitialized = false;
        }
    }

}