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
        public const float ExplosionRange = 5f;

        public override void SetupTasks(EnemyController controller)
        {
            base.SetupTasks(controller);

            Patrol.DistanceThreshold = ExplosionRange;
            Chase.DistanceThreshold = ExplosionRange;

            ExplodingCombat.Target = Target;
            ExplodingCombat.InterruptedEvent.AddListener(StartChase);
            ExplodingCombat.FinishedEvent.AddListener(StartWait);

            Wait.Target = Target;
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

        private void StartWait(EnemyController controller)
        {
            CurrentTask = Wait;
        }

        private ExplodeTask ExplodingCombat { get; set; } = new ExplodeTask();

        private WaitTask Wait { get; set; } = new WaitTask();
    }
}
