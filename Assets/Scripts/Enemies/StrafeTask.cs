using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Task representing movement around the specified Target.
    /// </summary>
    public class StrafeTask : Task
    {
        public ITarget Target { get; set; } = null;

        public EnemyController.StrafeDirection StrafeDirection = EnemyController.StrafeDirection.Clockwise;
        
        public float DistanceThreshold { get; set; } = 15f;

        public StrafeTask() {}
        
        public StrafeTask(ITarget target)
        {
            Target = target;
        }

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
                controller.UpdateStrafe(targetPosition, StrafeDirection);
            }
            else
            {
                InterruptedEvent.Invoke(controller);
            }
        }
    }
}