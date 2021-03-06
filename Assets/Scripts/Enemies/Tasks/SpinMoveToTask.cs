using UnityEngine;

namespace Enemies.Tasks
{
    /// <summary>
    /// Task representing moving to specified Target while spinning.
    /// </summary>
    public class SpinMoveToTask : MoveToTask
    {
        public EnemyController.RotationDirection SpinDirection = EnemyController.RotationDirection.Clockwise;
        
        public override void ExecuteUpdate(EnemyController controller)
        {
            (Vector3 currentPosition, Vector3 targetPosition) = CalculatePositions(controller);

            if (Vector3.Distance(currentPosition, targetPosition) > DistanceThreshold)
            {
                controller.UpdateSpinMoveTo(targetPosition, SpinDirection);
            }
            else
            {
                FinishedEvent.Invoke(controller);
            }
        }
    }
}