using UnityEngine;

namespace Enemies.Tasks
{
    /// <summary>
    /// Task representing movement around the specified Target.
    /// </summary>
    public class StrafeTask : Task
    {
        public EnemyController.RotationDirection StrafeDirection = EnemyController.RotationDirection.Clockwise;
        
        public float DistanceThreshold { get; set; } = 15f;

        public override void ExecuteUpdate(EnemyController controller)
        {
            (Vector3 currentPosition, Vector3 targetPosition) = CalculatePositions(controller);
            
            if (Vector3.Distance(currentPosition, targetPosition) <= DistanceThreshold)
            {
                controller.UpdateStrafe(targetPosition, StrafeDirection);
            }
            else
            {
                controller.ResetRigidbodyVelocity();
                controller.InstantlyRotateTo(targetPosition);
                InterruptedEvent.Invoke(controller);
            }
        }
    }
}
