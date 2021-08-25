using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    /// <summary>
    /// Task allowing deferred actions
    /// </summary>
    public class DeferredTask : MonoBehaviour
    {
        public Task task = null;

        public EnemyController controller = null;

        public float TimeToWait
        {
            get => timeToWait;
            set { timeToWait = value; timeRemaining = value; }
        }

        public bool shouldRun = false;

        public bool shouldLoop = false;

        public DeferredTask(Task task, EnemyController controller, float timeToWait, bool shouldRun, bool shouldLoop)
        {
            this.task = task;
            this.controller = controller;
            this.timeToWait = timeToWait;
            this.shouldRun = shouldRun;
            this.shouldLoop = shouldLoop;
            this.timeRemaining = timeToWait;
        }

        void FixedUpdate()
        {
            if (shouldRun)
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
                        timeRemaining = timeToWait;
                    }
                    else
                    {
                        timeRemaining = 0f;
                        shouldRun = false;
                    }
                }
            }
        }

        private float timeToWait = 0f;

        private float timeRemaining = 0f;
    }
}