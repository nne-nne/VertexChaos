using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Class which controls the Enemies' behavior, including locomotion and interaction with player's pawn.
    /// </summary>
    public class EnemyController : PlayerController
    {
        /// <summary> BehaviorSequence controls EnemyController actions' execution flow. </summary>
        public BehaviorSequence Behavior { get; set; } = new BehaviorSequence();

        public void UpdateMoveTo(Vector3 targetPosition)
        {
            Vector3 currentPosition = transform.position;
            
            Direction = new Vector2(targetPosition.x - currentPosition.x,
                targetPosition.z - currentPosition.z).normalized;
            
            UpdateMovementSpeed();
            UpdateRotation();
        }

        public void UpdateStrafe(Vector3 targetPosition)
        {
            Vector3 currentPosition = transform.position;
            Vector3 directionVector = new Vector3(targetPosition.x - currentPosition.x,
                0f, targetPosition.z - currentPosition.z).normalized;
            Vector3 rotatedDirectionVector = Quaternion.AngleAxis(90f, Vector3.up) * directionVector;
            
            Direction = new Vector2(rotatedDirectionVector.x, rotatedDirectionVector.z);
            
            UpdateMovementSpeed();
            UpdateRotationToFace(directionVector);
        }

        public void Wait()
        {
            Direction = Vector2.zero;
            UpdateMovementSpeed();
            UpdateRotation();
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            if (Behavior != null)
            {
                Behavior.UpdateTaskExecution(this);
            }
            else
            {
                Wait();
            }
        }
    }
}