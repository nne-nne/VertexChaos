using UnityEngine;

namespace Enemies.Tasks
{
    /// <summary>
    /// Task representing movement around the specified Target while spinning.
    /// </summary>
    public class SpinStrafeTask : StrafeTask
    {
        public EnemyController.RotationDirection SpinDirection = EnemyController.RotationDirection.Clockwise;

        public override void ExecuteUpdate(EnemyController controller)
        {
            Vector3 currentPosition = controller.transform.position;
            Vector3 targetPosition = currentPosition;
            if (Target != null) { targetPosition = Target.GetPosition(); }
            
            if (Vector3.Distance(currentPosition, targetPosition) <= DistanceThreshold)
            {
                controller.UpdateSpinStrafe(targetPosition, StrafeDirection, SpinDirection);
            }
            else
            {
                InterruptedEvent.Invoke(controller);
            }
        }
    }
}