using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Enemies.Tasks;

namespace Enemies.Behaviors
{
    /// <summary>
    /// BehaviorSequence is a collection of Tasks and logic controlling blending between them.
    /// Default behavior is agile strafing around player's pawn.
    /// Can be freely extended to support other behaviors.
    /// </summary>
    public class BehaviorSequence : MonoBehaviour
    {
        public ITarget Target
        {
            get => target;
            set
            {
                target = value;
                
                if (CurrentTask != null)
                {
                    CurrentTask.Target = target;
                }

                if (ParallelTask != null && ParallelTask.task != null)
                {
                    ParallelTask.task.Target = target;
                }
            }
        }

        public void UpdateTaskExecution(EnemyController controller)
        {
            CurrentTask?.ExecuteUpdate(controller);
        }

        public void InterruptCurrentTask(EnemyController controller)
        {
            CurrentTask?.InterruptedEvent?.Invoke(controller);
        }

        public void StopAllTasks()
        {
            CurrentTask = null;
            if (ParallelTask)
            {
                ParallelTask.shouldExecute = false;
            }
        }

        public virtual void SetupTasks(EnemyController controller)
        {
            Patrol.Target = Target;
            Patrol.FinishedEvent.AddListener(StartCombat);
            
            Combat.Target = Target;
            Combat.InterruptedEvent.AddListener(StartChase);
            Combat.FinishedEvent.AddListener(StartPatrol);

            Chase.Target = Target;
            Chase.FinishedEvent.AddListener(StartCombat);

            Shoot.Target = Target;
            
            ParallelTask.task = Shoot;
            ParallelTask.controller = controller;
            ParallelTask.TimeToWait = controller.timeBetweenShots;
            
            Wait.Target = Target;
            
            Target.GetDeathEvent().AddListener(OnTargetDeath);

            StartPatrol(controller);
        }

        protected MoveToTask Patrol { get; set; } = new MoveToTask();

        protected StrafeTask Combat { get; set; } = new StrafeTask();

        protected MoveToTask Chase { get; set; } = new MoveToTask();

        protected ShootTask Shoot { get; set; } = new ShootTask();
        
        protected WaitTask Wait { get; set; } = new WaitTask();

        protected virtual void StartPatrol(EnemyController controller)
        {
            Patrol.DistanceThreshold = Random.Range(controller.minTargetDistance, controller.maxTargetDistance);
            
            CurrentTask = Patrol;
            
            ParallelTask.shouldExecute = false;
        }

        protected virtual void StartCombat(EnemyController controller)
        {
            Combat.DistanceThreshold = controller.maxTargetDistance + controller.maxStrafeDistanceBias;
            Combat.StrafeDirection = Random.Range(0f, 1f) < 0.5f ?
                EnemyController.RotationDirection.Clockwise : EnemyController.RotationDirection.Counterclockwise;
            
            CurrentTask = Combat;
            
            ParallelTask.shouldExecute = true;
        }

        protected virtual void StartChase(EnemyController controller)
        {
            Chase.DistanceThreshold = Random.Range(controller.minTargetDistance, controller.maxTargetDistance);
            
            CurrentTask = Chase;

            ParallelTask.shouldExecute = true;
        }
        
        protected virtual void StartWait(EnemyController controller)
        {
            CurrentTask = Wait;
            
            ParallelTask.shouldExecute = false;
        }

        /// <summary> Task executed each frame. </summary>
        protected Task CurrentTask { get; set; } = null;

        /// <summary> Task executed continuously or periodically while CurrentTask executes. </summary>
        protected DeferredTask ParallelTask { get; set; } = null;

        private ITarget target = new PointTarget();

        private void Awake()
        {
            ParallelTask = gameObject.AddComponent<DeferredTask>();
            ParallelTask.shouldExecute = false;
            ParallelTask.shouldLoop = true;
        }

        private void OnTargetDeath()
        {
            CurrentTask = Wait;
            ParallelTask.shouldExecute = false;
        }
    }
}
