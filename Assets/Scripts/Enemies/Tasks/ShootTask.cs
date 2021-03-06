using UnityEngine;

namespace Enemies.Tasks
{
    /// <summary>
    /// Task representing shooting at specified target.
    /// </summary>
    public class ShootTask : Task
    {
        public override void ExecuteUpdate(EnemyController controller)
        {
            controller.Shoot();
            FinishedEvent.Invoke(controller);
        }
    }
}
