using System;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Class which controls the Enemies' behavior, including locomotion and interaction with player's pawn.
    /// </summary>
    public class EnemyController : PlayerController
    {
        /// <summary> BehaviorSequence controls EnemyController actions' execution flow. </summary>
        public BehaviorSequence Behavior { get; set; } = null;

        public float minTargetDistance = 10f;

        public float maxTargetDistance = 30f;

        public float maxStrafeDistanceBias = 5f;

        public float timeBetweenShots = 2f;

        public void UpdateMoveTo(Vector3 targetPosition)
        {
            Vector3 currentPosition = transform.position;
            
            Direction = new Vector2(targetPosition.x - currentPosition.x,
                targetPosition.z - currentPosition.z).normalized;
            
            UpdateMovementSpeed();
            UpdateRotation();
        }

        public void UpdateStrafe(Vector3 targetPosition, StrafeDirection strafeDirection)
        {
            Vector3 currentPosition = transform.position;
            Vector3 directionVector = new Vector3(targetPosition.x - currentPosition.x,
                0f, targetPosition.z - currentPosition.z).normalized;

            Vector3 rotatedDirectionVector = strafeDirection switch
            {
                StrafeDirection.Clockwise => Quaternion.AngleAxis(-90f, Vector3.up) * directionVector,
                StrafeDirection.Counterclockwise => Quaternion.AngleAxis(90f, Vector3.up) * directionVector,
                _ => new Vector3()
            };

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

        public void Shoot()
        {
            Debug.Log("Pium!");
        }

        protected override void Awake()
        {
            base.Awake();
            Behavior = gameObject.AddComponent<BehaviorSequence>();
            affiliation = Affiliation.Enemy;
            PlayerController player = GameObject.Find(PlayerName).GetComponent<PlayerController>();
            if (player is ITarget target && Behavior != null)
            {
                Behavior.Target = target;
                Behavior.SetupTasks(this);
            }
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
        
        public enum StrafeDirection
        {
            Clockwise,
            Counterclockwise
        }
    }
}