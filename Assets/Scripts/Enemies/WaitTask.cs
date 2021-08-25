using UnityEngine;

namespace Enemies
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
