using UnityEngine;

namespace Enemies
{
    public class ShootTask : Task
    {
        public ITarget Target { get; set; } = null;

        public ShootTask()
        {
        }

        public ShootTask(ITarget target)
        {
            Target = target;
        }

        public override void ExecuteUpdate(EnemyController controller)
        {
            controller.Shoot();
            FinishedEvent.Invoke(controller);
        }
    }
}