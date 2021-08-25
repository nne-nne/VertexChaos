using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Class which controls the Enemies' behaviour, including locomotion and interaction with player's pawn.
    /// </summary>
    public class EnemyController : PlayerController
    {
        protected override void Awake()
        {
            base.Awake();
            CurrentTask = new MoveToTask(new PointTarget(20f, 0f, -20f));
        }

        public void UpdateMoveTo(Vector3 targetPosition)
        {
            Vector3 currentPosition = transform.position;
            Direction = new Vector2(targetPosition.x - currentPosition.x,
                targetPosition.z - currentPosition.z).normalized;
            UpdateMovementSpeed();
            UpdateRotation();
        }

        public void Wait()
        {
            Direction = Vector2.zero;
            UpdateMovementSpeed();
            UpdateRotation();
        }

        protected override void Update()
        {
            if (CurrentTask != null)
            {
                CurrentTask.ExecuteUpdate(this);
            }
            else
            {
                Wait();
            }
        }
        
        /// <summary> Current Task keeps information about actions which EnemyController executes. </summary>
        public Task CurrentTask { get; set; } = null;

    }
}