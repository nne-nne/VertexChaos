using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Class representing behavior of spinning enemy unit.
    /// </summary>
    public class SpinningBehavior : BehaviorSequence
    {
        SpinningBehavior()
        {
            EnemyController.RotationDirection spinDirection = EnemyController.RotationDirection.Counterclockwise;

            SpinStrafeTask spinStrafeTask = new SpinStrafeTask();
            spinStrafeTask.SpinDirection = spinDirection;
            Combat = spinStrafeTask;

            SpinMoveToTask spinMoveToTask = new SpinMoveToTask();
            spinMoveToTask.SpinDirection = spinDirection;
            Chase = spinMoveToTask;
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