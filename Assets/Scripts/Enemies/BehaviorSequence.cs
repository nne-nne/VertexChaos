using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    /// <summary>
    /// BehaviorSequence is a collection of Tasks and logic controlling blending between them.
    /// </summary>
    public class BehaviorSequence : MonoBehaviour
    {
        public MoveToTask Patrol { get; set; } = new MoveToTask();

        public StrafeTask Combat { get; set; } = new StrafeTask();

        public MoveToTask Chase { get; set; } = new MoveToTask();

        public ShootTask Shoot { get; set; } = new ShootTask();
        
        public ITarget Target { get; set; } = new PointTarget();

        public BehaviorSequence() {}

        private void Awake()
        {
            ParallelTask = gameObject.AddComponent<DeferredTask>();
        }

        public void UpdateTaskExecution(EnemyController controller)
        {
            CurrentTask?.ExecuteUpdate(controller);
        }

        public void InterruptCurrentTask(EnemyController controller)
        {
            CurrentTask?.InterruptedEvent?.Invoke(controller);
        }

        public void SetupTasks(EnemyController controller)
        {
            Patrol.Target = Target;
            Patrol.FinishedEvent.AddListener(StartCombat);
            
            Combat.Target = Target;
            Combat.FinishedEvent.AddListener(StartPatrol);
            Combat.InterruptedEvent.AddListener(StartChase);
            
            Chase.Target = Target;
            Chase.FinishedEvent.AddListener(StartCombat);

            Shoot.Target = Target;
            
            ParallelTask.task = Shoot;
            ParallelTask.controller = controller;
            ParallelTask.TimeToWait = controller.timeBetweenShots;
            ParallelTask.shouldRun = true;
            ParallelTask.shouldLoop = true;
            
            StartPatrol(controller);
        }

        public void StartPatrol(EnemyController controller)
        {
            Patrol.DistanceThreshold = Random.Range(controller.minTargetDistance, controller.maxTargetDistance);
            
            CurrentTask = Patrol;
            
            ParallelTask.shouldRun = false;
        }

        public void StartCombat(EnemyController controller)
        {
            Combat.DistanceThreshold = controller.maxTargetDistance + controller.maxStrafeDistanceBias;
            Combat.StrafeDirection = Random.Range(0f, 1f) < 0.5f ?
                EnemyController.StrafeDirection.Clockwise : EnemyController.StrafeDirection.Counterclockwise;
            
            CurrentTask = Combat;
            
            ParallelTask.shouldRun = true;
        }

        public void StartChase(EnemyController controller)
        {
            Chase.DistanceThreshold = Random.Range(controller.minTargetDistance, controller.maxTargetDistance);
            
            CurrentTask = Chase;

            ParallelTask.shouldRun = true;
        }

        /// <summary> Current Task keeps information about actions which EnemyController executes. </summary>
        private Task CurrentTask { get; set; } = null;

        /// <summary> Parallel Task is optional task executed while CurrentTask is executing. </summary>
        private DeferredTask ParallelTask { get; set; } = null;
    }
}