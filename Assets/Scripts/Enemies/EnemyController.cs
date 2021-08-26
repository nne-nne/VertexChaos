using System.Collections.Generic;
using UnityEngine;
using Enemies.Behaviors;

namespace Enemies
{
    /// <summary>
    /// Class which controls the Enemies' actions, including locomotion and interaction with player's pawn.
    /// </summary>
    public class EnemyController : PlayerController
    {
        public enum RotationDirection
        {
            Clockwise,
            Counterclockwise
        }

        public enum BehaviorType
        {
            Simple,
            Agile,
            Exploding,
            [InspectorName("Spin2Win")]
            Spinning
        }

        public float minTargetDistance = 10f;

        public float maxTargetDistance = 30f;

        public float maxStrafeDistanceBias = 5f;

        public float timeBetweenShots = 2f;

        public BehaviorType behaviorType = BehaviorType.Simple;

        public void UpdateMoveTo(Vector3 targetPosition)
        {
            Vector3 currentPosition = transform.position;

            Direction = new Vector2(targetPosition.x - currentPosition.x,
                targetPosition.z - currentPosition.z).normalized;

            UpdateMovementSpeed();
            UpdateRotation();
        }

        public void UpdateStrafe(Vector3 targetPosition,
            RotationDirection strafeDirection = RotationDirection.Clockwise)
        {
            Vector3 currentPosition = transform.position;
            Vector3 directionVector = new Vector3(targetPosition.x - currentPosition.x,
                0f, targetPosition.z - currentPosition.z).normalized;

            Vector3 rotatedDirectionVector = strafeDirection switch
            {
                RotationDirection.Clockwise => Quaternion.AngleAxis(-90f, Vector3.up) * directionVector,
                RotationDirection.Counterclockwise => Quaternion.AngleAxis(90f, Vector3.up) * directionVector,
                _ => new Vector3()
            };

            Direction = new Vector2(rotatedDirectionVector.x, rotatedDirectionVector.z);

            UpdateMovementSpeed();
            UpdateRotationToFace(directionVector);
        }

        public void UpdateKeepRotatedTowards(Vector3 targetPosition)
        {
            Vector3 currentPosition = transform.position;
            Vector3 directionVector = new Vector3(targetPosition.x - currentPosition.x,
                0f, targetPosition.z - currentPosition.z).normalized;

            Direction = Vector2.zero;

            UpdateMovementSpeed();
            UpdateRotationNoVelocity(directionVector);
        }

        public void InstantlyRotateTo(Vector3 targetPosition)
        {
            Vector3 currentPosition = transform.position;
            Vector3 directionVector = new Vector3(targetPosition.x - currentPosition.x,
                0f, targetPosition.z - currentPosition.z).normalized;
            
            SetRotation(directionVector);
        }

        public void UpdateSpinMoveTo(Vector3 targetPosition, RotationDirection spinDirection)
        {
            Vector3 currentPosition = transform.position;
            Vector3 directionVector = new Vector3(targetPosition.x - currentPosition.x,
                0f, targetPosition.z - currentPosition.z).normalized;

            Vector3 spinDirectionVector = spinDirection switch
            {
                RotationDirection.Clockwise => Quaternion.AngleAxis(-90f, Vector3.up) * transform.forward,
                RotationDirection.Counterclockwise => Quaternion.AngleAxis(90f, Vector3.up) * transform.forward,
                _ => new Vector3()
            };

            Direction = new Vector2(directionVector.x, directionVector.z);

            UpdateMovementSpeed();
            UpdateRotationToFace(spinDirectionVector);
            ResetRigidbodyVelocity(false, true);
        }

        public void UpdateSpinStrafe(Vector3 targetPosition,
            RotationDirection strafeDirection = RotationDirection.Clockwise,
            RotationDirection spinDirection = RotationDirection.Clockwise)
        {
            Vector3 currentPosition = transform.position;
            Vector3 directionVector = new Vector3(targetPosition.x - currentPosition.x,
                0f, targetPosition.z - currentPosition.z).normalized;

            Vector3 rotatedDirectionVector = strafeDirection switch
            {
                RotationDirection.Clockwise => Quaternion.AngleAxis(-90f, Vector3.up) * directionVector,
                RotationDirection.Counterclockwise => Quaternion.AngleAxis(90f, Vector3.up) * directionVector,
                _ => new Vector3()
            };
            
            Vector3 spinDirectionVector = spinDirection switch
            {
                RotationDirection.Clockwise => Quaternion.AngleAxis(-90f, Vector3.up) * transform.forward,
                RotationDirection.Counterclockwise => Quaternion.AngleAxis(90f, Vector3.up) * transform.forward,
                _ => new Vector3()
            };

            Direction = new Vector2(rotatedDirectionVector.x, rotatedDirectionVector.z);

            UpdateMovementSpeed();
            UpdateRotationToFace(spinDirectionVector);
            ResetRigidbodyVelocity(false, true);
        }

        /// <remarks> Add shooting mechanic </remarks> 
        public void Shoot()
        {
            GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;

                BulletSc bulletSc = bullet.GetComponent<BulletSc>();

                if (bulletSc != null)
                {
                    //following method sets bullet active in hierarchy
                    bulletSc.Activate();
                    bulletSc.AddModifiers(bulletModifiers);
                    bulletSc.Shoot();

                }
            }
        }

        /// <remarks> Add exploding mechanic </remarks> 
        public void Explode()
        {
            Debug.Log("Bum!");
        }

        public void Wait()
        {
            Direction = Vector2.zero;
            
            UpdateMovementSpeed();
            UpdateRotation();
        }

        /// <summary> BehaviorSequence controls EnemyController actions' execution flow. </summary>
        protected BehaviorSequence Behavior { get; set; } = null;

        protected override void Awake()
        {
            base.Awake();

            affiliation = Affiliation.Enemy;
            InitializeBehavior();
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

        private List<BulletModifier> bulletModifiers = new List<BulletModifier>();
        
        private void InitializeBehavior()
        {
            Behavior = behaviorType switch
            {
                BehaviorType.Simple => gameObject.AddComponent<SimpleBehavior>(),
                BehaviorType.Agile => gameObject.AddComponent<BehaviorSequence>(),
                BehaviorType.Exploding => gameObject.AddComponent<ExplodingBehavior>(),
                BehaviorType.Spinning => gameObject.AddComponent<SpinningBehavior>(),
                _ => Behavior
            };

            PlayerController player = GameObject.Find(PlayerName).GetComponent<PlayerController>();
            if (player is ITarget target && Behavior != null)
            {
                Behavior.Target = target;
                Behavior.SetupTasks(this);
            }
        }
    }
}
