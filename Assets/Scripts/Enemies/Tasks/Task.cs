using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Tasks
{
    /// <summary>
    /// Task is a series of instructions executed by EnemyController.
    /// </summary>
    public abstract class Task
    {
        public abstract void ExecuteUpdate(EnemyController controller);

        public UnityEvent<EnemyController> InterruptedEvent { get; set; } = new UnityEvent<EnemyController>();
        
        public UnityEvent<EnemyController> FinishedEvent { get; set; } = new UnityEvent<EnemyController>();
        
        public ITarget Target { get; set; } = null;

        protected (Vector3 currentPosition, Vector3 targetPosition) CalculatePositions(EnemyController controller)
        {
            Vector3 currentPosition = controller.transform.position;
            Vector3 targetPosition = currentPosition;
            if (Target != null) { targetPosition = Target.GetPosition(); }
            
            return (currentPosition, targetPosition);
        }
    }
}
