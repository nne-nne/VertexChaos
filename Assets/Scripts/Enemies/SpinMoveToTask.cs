using UnityEngine;

namespace Enemies
{
    public class SpinMoveToTask : MoveToTask
    {
        public EnemyController.RotationDirection SpinDirection = EnemyController.RotationDirection.Clockwise;
        
        public override void ExecuteUpdate(EnemyController controller)
        {
            Vector3 currentPosition = controller.transform.position;
            Vector3 targetPosition = currentPosition;

            if (Target != null)
            {
                targetPosition = Target.GetPosition();
            }

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