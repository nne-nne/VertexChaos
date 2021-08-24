using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Task representing movement around the specified Target.
    /// </summary>
    public class StrafeTask : Task
    {
        public ITarget Target { get; set; } = null;

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
            
            controller.UpdateStrafe(targetPosition);
        }
    }
}