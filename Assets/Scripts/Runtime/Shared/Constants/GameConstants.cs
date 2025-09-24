using UnityEngine;

namespace Assets.Scripts.Runtime.Shared.Constants
{
    public static class GameConstants
    {
        public readonly static float BasketRadius = 0.3f;
        public readonly static float MissStrongDistance = 2.0f;
        public readonly static float MissWeakDistance = 1.0f;
        public readonly static float MinFreeThrowLineDistance = -4.0f;
        public readonly static float MinShotTimeToTarget = 1.0f;
        public readonly static float MaxShotTimeToTarget = 2.0f;
        public readonly static float TimeClampFactor = 4.0f;
        public readonly static float BackClampFactor = 8.0f;
        public readonly static float MaxSwipeDistance = 800f; 
        public readonly static float MinSwipeSpeed = 100f;
        public readonly static float SwipeTimeWindow = 0.8f;
        public const float BallRotationMultiplier = 1.5f;
        public const float MaxAngularVelocity = 12.0f;
        public readonly static Vector3 BallOffset = new Vector3(0.15f, 1.6f, 0.3f);

        public readonly static float AmbientVolume = 1.0f;
        public readonly static float MusicVolume = 0.4f;
        public readonly static string MusicSound = "basketball-music";
        public readonly static string NetSound = "basketball-net";
        public readonly static string ThrowSound = "basketball-throw";
        public readonly static string BackbordSound = "basketball-backboard";
        public readonly static string AmbientSound = "basketball-ambient";
        public readonly static string BuzzerGameOverSound = "basketball-buzzer-game-over";
        public readonly static string RefereeWhistleSound = "basketball-referee-whistle";

        public const string VContainer_SFXAudioSourceKey = "SFX_AudioSource";
        public const string VContainer_MusicAudioSourceKey = "Music_AudioSource";

        public static int GetRandomEvenOdd() 
        {
            return Random.value < 0.5f ? -1 : 1; 
        }
    }
}