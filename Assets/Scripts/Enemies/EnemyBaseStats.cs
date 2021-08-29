using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Struct to hold default enemy stats.
    /// </summary>
    public struct EnemyBaseStats
    {
        public float health;
        
        public float maxHealth;
        
        public float minTargetDistance;

        public float maxTargetDistance;

        public float maxStrafeDistanceBias;

        public float timeBetweenShots;

        public EnemyController.BehaviorType behaviorType;

        public Vector3 localScale;

        public float maxMovSpeed;
        
        public float acceleration;
        
        public float angularSpeed;
    }
}