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
        public readonly static float MaxSwipeDistance = 500f; 
        public readonly static float MinSwipeSpeed = 100f;
        public readonly static float SwipeTimeWindow = 2f;
        public readonly static int FireballStreakThreshold = 3;
        public readonly static Vector3 BallOffset = new Vector3(0.15f, 1.6f, 0.3f);
        
        public static int GetRandomEvenOdd() 
        {
            return Random.value < 0.5f ? -1 : 1; 
        }
    }
}