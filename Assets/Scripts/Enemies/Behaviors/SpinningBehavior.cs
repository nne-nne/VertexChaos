using UnityEngine;
using Enemies.Behaviors;
using Enemies.Tasks;

namespace Enemies
{
    /// <summary>
    /// Class representing behavior of spinning enemy unit.
    /// </summary>
    public class SpinningBehavior : BehaviorSequence
    {
        public SpinningBehavior()
        {
            Combat = new SpinStrafeTask();
            Chase = new SpinMoveToTask();
        }

        public override void SetupTasks(EnemyController controller)
        {
            base.SetupTasks(controller);
            
            EnemyController.RotationDirection spinDirection = Random.Range(0f, 1f) < 0.5f ?
                EnemyController.RotationDirection.Clockwise : EnemyController.RotationDirection.Counterclockwise;

            if (Combat is SpinStrafeTask spinStrafeTask)
            {
                spinStrafeTask.SpinDirection = spinDirection;
            }

            if (Chase is SpinMoveToTask spinMoveToTask)
            {
                spinMoveToTask.SpinDirection = spinDirection;
            }
        }
    }
}