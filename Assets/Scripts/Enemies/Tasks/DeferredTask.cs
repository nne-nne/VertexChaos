using UnityEngine;

namespace Enemies.Tasks
{
    /// <summary>
    /// Task for actions that should be deferred or happen periodically.
    /// </summary>
    public class DeferredTask : MonoBehaviour
    {
        public Task task = null;

        public EnemyController controller = null;

        public float TimeToWait
        {
            get => timeToWait;
            set { timeToWait = value; timeRemaining = value + Randomize(); }
        }

        public bool shouldExecute = false;

        public bool shouldLoop = true;
        
        public float randomDeviation = 0f;

        private void FixedUpdate()
        {
            if (shouldExecute)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    task?.ExecuteUpdate(controller);
                    
                    if (shouldLoop)
                    {
                        timeRemaining = timeToWait + Randomize();
                    }
                    else
                    {
                        timeRemaining = 0f;
                        shouldExecute = false;
                    }
                }
            }
        }

        private float timeToWait = 0f;

        private float timeRemaining = 0f;

        private float Randomize()
        {
            return Random.Range(0f, randomDeviation);
        }
    }
}
