using UnityEngine;
using Enemies.Behaviors;
using Enemies.Tasks;

namespace Enemies
{
    /// <summary>
    /// Class representing behavior of simple enemy unit.
    /// </summary>
    public class SimpleBehavior : BehaviorSequence
    {
        public float RandomDeviation { get; set; } = 0.0f;
        
        public override void SetupTasks(EnemyController controller)
        {
            base.SetupTasks(controller);

            SimpleCombat.Target = Target;
            SimpleCombat.InterruptedEvent.AddListener(StartChase);

            ParallelTask.randomDeviation = RandomDeviation;
            ParallelTask.shouldExecute = true;
        }

        protected override void StartCombat(EnemyController controller)
        {
            base.StartCombat(controller);
            
            SimpleCombat.DistanceThreshold = controller.maxTargetDistance + controller.maxStrafeDistanceBias;

            CurrentTask = SimpleCombat;
            
            ParallelTask.shouldExecute = true;
        }

        private KeepRotatedTowardsTask SimpleCombat { get; set; } = new KeepRotatedTowardsTask();
    }
}
