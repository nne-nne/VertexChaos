using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// WASD/arrows controller for player movement
/// 
/// </summary>
public class PlayerController : MonoBehaviour, ITarget
{
    [SerializeField] private float maxMovSpeed, acceleration, angularSpeed;

    private Rigidbody rb;
    private Vector2 speed, direction;
    private Quaternion rotation;
    
    protected Affiliation affiliation = Affiliation.Player;

    [FormerlySerializedAs("Health")] [SerializeField]
    public float health = 10f;

    [FormerlySerializedAs("MaxHealth")] [SerializeField]
    public float maxHealth = 10f;

    public static string PlayerName { get; } = "Player";
    
    public ParticleSystem deathParticles = null;

    public void ResetRigidbodyVelocity(bool resetLinear = true, bool resetAngular = true)
    {
        if (resetLinear) { rb.velocity = Vector3.zero; }
        if (resetAngular) { rb.angularVelocity = Vector3.zero; }
    }
    
    public UnityEvent DeathEvent { get; set; } = new UnityEvent();

    /// <remarks> For cosmetics or UI </remarks>
    public UnityEvent<float> ReceiveDamageEvent { get; set; }= new UnityEvent<float>();

    public List<AudioClip> deathSounds = new List<AudioClip>();
    
    public List<AudioClip> shootSounds = new List<AudioClip>();

    protected Vector2 Direction
    {
        get => direction;
        set => direction = value;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        gameObject.GetComponentInChildren<Collider>().tag = PlayerName;
        DeathEvent.AddListener(Die);
        audioSource = GetComponent<AudioSource>();
        
        CannonController cannonController = GetComponentInChildren<CannonController>();
        if (cannonController != null && cannonController.ShootEvent != null)
        {
            cannonController.ShootEvent.AddListener(PlayShootSound);
        }
        
        gameObject.tag = PlayerName;
    }

    protected void UpdateMovementSpeed()
    {
        if (direction != Vector2.zero) direction.Normalize();

        speed = Vector2.Lerp(speed, direction * maxMovSpeed, acceleration * Time.deltaTime);
    }

    protected void UpdateRotation()
    {
        Quaternion target;

        if (rb.velocity != Vector3.zero)
        {
            target = Quaternion.LookRotation(rb.velocity.normalized);
        }
        else
        {
            target = transform.rotation;
        }

        float rotationSpeed = angularSpeed * rb.velocity.magnitude / maxMovSpeed;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed);
    }

    protected void UpdateRotationToFace(Vector3 targetDirection)
    {
        Quaternion target;

        if (rb.velocity != Vector3.zero)
        {
            target = Quaternion.LookRotation(targetDirection.normalized);
        }
        else
        {
            target = transform.rotation;
        }

        float rotationSpeed = angularSpeed * rb.velocity.magnitude / maxMovSpeed;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed);
    }

    protected void UpdateRotationNoVelocity(Vector3 targetDirection)
    {
        Quaternion target = Quaternion.LookRotation(targetDirection.normalized);

        float rotationSpeed = angularSpeed / maxMovSpeed;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed);
    }

    protected void SetRotation(Vector3 targetDirection)
    {
        Quaternion target = Quaternion.LookRotation(targetDirection.normalized);

        const float fullAngle = 180f;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, target, fullAngle);
    }

    protected virtual void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        UpdateMovementSpeed();
        UpdateRotation();
    }
    
    public virtual void ReceiveDamage(float amount)
    {
        ReceiveDamageEvent.Invoke(amount);

        if (health - amount <= 0f)
        {
            health = 0f;
            DeathEvent.Invoke();
        }
        else
        {
            health -= amount;
        }
    }

    protected virtual void Die()
    {
        PlayDeathSound();
        if (deathParticles)
        {
            deathParticles.Play(true);
        }
        gameObject.SetActive(false);
    }

    protected virtual void PlayDeathSound()
    {
        if (audioSource != null && deathSounds != null && deathSounds.Count > 0)
        {
            Debug.Log("Death Sound");
            int audioIndex = Random.Range(0, deathSounds.Count);
            AudioClip audioClip = deathSounds[audioIndex];
            audioSource.PlayOneShot(audioClip);
        }
    }
    
    protected virtual void PlayShootSound()
    {
        if (audioSource != null && shootSounds != null && shootSounds.Count > 0)
        {
            Debug.Log("Shoot Sound");
            int audioIndex = Random.Range(0, shootSounds.Count);
            AudioClip audioClip = shootSounds[audioIndex];
            audioSource.PlayOneShot(audioClip);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(speed.x, 0f, speed.y);
    }

    private AudioSource audioSource = null;
    
    Vector3 ITarget.GetPosition()
    {
        return transform.position;
    }

    UnityEvent ITarget.GetDeathEvent()
    {
        return DeathEvent;
    }
}
