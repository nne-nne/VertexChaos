using UnityEngine;
using UnityEngine.Events;

namespace Enemies
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
    }
}
