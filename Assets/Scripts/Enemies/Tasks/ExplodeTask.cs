using UnityEngine;

namespace Enemies.Tasks
{
    /// <summary>
    /// Task representing exploding to damage specified target.
    /// </summary>
    public class ExplodeTask : Task
    {
        public override void ExecuteUpdate(EnemyController controller)
        {
            controller.Explode();
            FinishedEvent.Invoke(controller);
        }
    }
}