using UnityEngine;

namespace Enemies.Tasks
{
    /// <summary>
    /// Task representing continuous movement toward a certain target.
    /// </summary>
    public class MoveToTask : Task
    {
        public float DistanceThreshold { get; set; } = 1f;

        public override void ExecuteUpdate(EnemyController controller)
        {
            (Vector3 currentPosition, Vector3 targetPosition) = CalculatePositions(controller);

            if (Vector3.Distance(currentPosition, targetPosition) > DistanceThreshold)
            {
                controller.UpdateMoveTo(targetPosition);
            }
            else
            {
                FinishedEvent.Invoke(controller);
            }
        }
    }
}
