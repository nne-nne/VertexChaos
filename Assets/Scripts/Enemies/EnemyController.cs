using System;
using System.Collections.Generic;
using UnityEngine;
using Enemies.Behaviors;
using UnityEngine.Events;

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

        public static string EnemyName = "Enemy";

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
            Vector3 directionVector = CalculateDirection(targetPosition);

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
            Vector3 directionVector = CalculateDirection(targetPosition);

            Direction = Vector2.zero;

            UpdateMovementSpeed();
            UpdateRotationNoVelocity(directionVector);
        }

        public void InstantlyRotateTo(Vector3 targetPosition)
        {
            Vector3 directionVector = CalculateDirection(targetPosition);
            
            SetRotation(directionVector);
        }

        public void UpdateSpinMoveTo(Vector3 targetPosition, RotationDirection spinDirection)
        {
            Vector3 directionVector = CalculateDirection(targetPosition);

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
            Vector3 directionVector = CalculateDirection(targetPosition);

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
                    bulletSc.affiliation = affiliation;
                    bulletSc.Activate();
                    bulletSc.AddModifiers(bulletModifiers);
                    bulletSc.Shoot();

                    PlayShootSound();
                }
            }
        }
        
        public void Explode()
        {
            DeathEvent.Invoke();
        }

        public void Wait()
        {
            Direction = Vector2.zero;
            
            UpdateMovementSpeed();
            UpdateRotation();
        }

        public void ApplyEnemyModifiers()
        {
            foreach (var enemyModifier in enemyModifiers)
            {
                enemyModifier.create_effect(gameObject);
            }
        }

        /// <summary> BehaviorSequence controls EnemyController actions' execution flow. </summary>
        protected BehaviorSequence Behavior { get; set; } = null;

        protected override void Awake()
        {
            base.Awake();
            

            CopyToBaseStats();

            affiliation = Affiliation.Enemy;
            gameObject.tag = EnemyName;
            gameObject.GetComponentInChildren<Collider>().tag = EnemyName;
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
            
            /// DEBUG ONLY!!!
            if (Input.GetKeyDown(KeyCode.X))
                enemyModifiers.Add(new BigBadEnemyModifier());
        }

        protected override void Die()
        {
            Behavior.StopAllTasks();
            base.Die();
        }

        private List<BulletModifier> bulletModifiers = new List<BulletModifier>();

        private List<EnemyModifier> enemyModifiers = new List<EnemyModifier>();

        private EnemyBaseStats baseStats;

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

        private Vector3 CalculateDirection(Vector3 targetPosition)
        {
            Vector3 currentPosition = transform.position;
            return new Vector3(targetPosition.x - currentPosition.x,
                0f, targetPosition.z - currentPosition.z).normalized;
        }
        

        private void CopyToBaseStats()
        {
            baseStats.health = health;
            baseStats.maxHealth = maxHealth;
            baseStats.minTargetDistance = minTargetDistance;
            baseStats.maxTargetDistance = maxTargetDistance;
            baseStats.maxStrafeDistanceBias = maxStrafeDistanceBias;
            baseStats.timeBetweenShots = timeBetweenShots;
            baseStats.behaviorType = behaviorType;

            baseStats.localScale = transform.localScale;
        }

        private void CopyBaseStatsToActual()
        {
            health = baseStats.health;
            maxHealth = baseStats.maxHealth;
            minTargetDistance = baseStats.minTargetDistance;
            maxTargetDistance = baseStats.maxTargetDistance;
            maxStrafeDistanceBias = baseStats.maxStrafeDistanceBias;
            timeBetweenShots = baseStats.timeBetweenShots;
            behaviorType = baseStats.behaviorType;
            transform.localScale = baseStats.localScale;
        }

        private void OnEnable()
        {
            CopyBaseStatsToActual();
            ApplyEnemyModifiers();
            Behavior.SetupTasks(this);
        }

        public void AddModifier(EnemyModifier mod)
        {
            enemyModifiers.Add(mod);
        }
    }
}
