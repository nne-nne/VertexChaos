using UnityEngine;

namespace Enemies.Tasks
{
    /// <summary>
    /// Task representing waiting.
    /// </summary>
    public class WaitTask : Task
    {
        public override void ExecuteUpdate(EnemyController controller)
        {
            controller.Wait();
            FinishedEvent.Invoke(controller);
        }
    }
}
