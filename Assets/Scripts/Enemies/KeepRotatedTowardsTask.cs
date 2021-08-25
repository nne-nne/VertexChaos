using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Task representing continuous rotating to face a certain target.
    /// </summary>
    public class KeepRotatedTowardsTask : Task
    {
        public float DistanceThreshold { get; set; } = 1f;
        
        public override void ExecuteUpdate(EnemyController controller)
        {
            Vector3 currentPosition = controller.transform.position;
            Vector3 targetPosition = currentPosition;

            if (Target != null)
            {
                targetPosition = Target.GetPosition();
            }
            
            if (Vector3.Distance(currentPosition, targetPosition) <= DistanceThreshold)
            {
                controller.UpdateKeepRotatedTowards(targetPosition);
            }
            else
            {
                InterruptedEvent.Invoke(controller);
            }
        }
    }
}
