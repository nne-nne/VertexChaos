using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Task representing continuous movement toward a certain target.
    /// </summary>
    public class MoveToTask : Task
    {
        public MoveToTask(ITarget target)
        {
            Target = target;
            FinishedEvent.AddListener(OnFinished);
        }
        
        public ITarget Target { get; set; } = null;

        public float DistanceThreshold { get; set; } = 1f;
        
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
                controller.UpdateMoveTo(targetPosition);
            }
            else
            {
                FinishedEvent.Invoke(controller);
            }
        }

        private void OnFinished(EnemyController controller)
        {
            controller.CurrentTask = null;
        }
    }
}