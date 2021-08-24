using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// BehaviorSequence is a collection of Tasks and logic controlling blending between them.
    /// </summary>
    public class BehaviorSequence
    {
        public BehaviorSequence()
        {
            moveToTask.DistanceThreshold = 10f;
            moveToTask.FinishedEvent.AddListener((EnemyController controller) => CurrentTask = strafeTask);
            CurrentTask = moveToTask;
        }
        
        public void UpdateTaskExecution(EnemyController controller)
        {
            CurrentTask?.ExecuteUpdate(controller);
        }

        public void InterruptCurrentTask(EnemyController controller)
        {
            CurrentTask?.InterruptedEvent?.Invoke(controller);
        }

        /// <summary> Current Task keeps information about actions which EnemyController executes. </summary>
        private Task CurrentTask { get; set; } = null;
        
        private MoveToTask moveToTask = new MoveToTask(new PointTarget());

        private StrafeTask strafeTask = new StrafeTask(new PointTarget());
    }
}