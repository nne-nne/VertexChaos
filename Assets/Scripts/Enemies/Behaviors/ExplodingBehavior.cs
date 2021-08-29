using System;
using UnityEngine;
using Enemies.Behaviors;
using Enemies.Tasks;

namespace Enemies
{
    /// <summary>
    /// Class representing behavior of enemy unit trying to explode.
    /// </summary>
    public class ExplodingBehavior : BehaviorSequence
    {
        public const float ExplosionRange = 8f;

        public ExplodingBehavior()
        {
            Patrol = new SpinMoveToTask();
            Chase = new SpinMoveToTask();
        }

        public override void SetupTasks(EnemyController controller)
        {
            base.SetupTasks(controller);
            
            Patrol.DistanceThreshold = ExplosionRange;
            Chase.DistanceThreshold = ExplosionRange;

            ExplodingCombat.Target = Target;
            ExplodingCombat.InterruptedEvent.AddListener(StartChase);
            ExplodingCombat.FinishedEvent.AddListener(StartWait);
        }

        protected override void StartCombat(EnemyController controller)
        {
            base.StartCombat(controller);

            CurrentTask = ExplodingCombat;
            
            ParallelTask.shouldExecute = false;
        }

        protected override void StartChase(EnemyController controller)
        {
            base.StartChase(controller);

            ParallelTask.shouldExecute = false;
        }

        private ExplodeTask ExplodingCombat { get; set; } = new ExplodeTask();
    }
}
