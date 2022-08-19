using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float maxMovSpeed, acceleration, angularSpeed;

    private Rigidbody rb;
    private Vector2 speed, direction;
    private Quaternion rotation;
    
    public Affiliation affiliation = Affiliation.Player;

    [FormerlySerializedAs("Health")] [SerializeField]
    public float health = 10f;

    [FormerlySerializedAs("MaxHealth")] [SerializeField]
    public float maxHealth = 10f;

    public float invincibilityTime = 0f;
    private float lastDamage = 0f;

    public static string PlayerName { get; } = "Player";

    public float damageSignalizationTime = 0.1f;
    
    private Color damageColor = Color.white;

    public Texture damageTexture;

    public float healFrac;

    public void ResetRigidbodyVelocity(bool resetLinear = true, bool resetAngular = true)
    {
        if (resetLinear) { rb.velocity = Vector3.zero; }
        if (resetAngular) { rb.angularVelocity = Vector3.zero; }
    }
    
    public UnityEvent DeathEvent { get; set; } = new UnityEvent();

    /// <remarks> For cosmetics or UI </remarks>
    public UnityEvent<float> ReceiveDamageEvent { get; set; } = new UnityEvent<float>();

    public List<AudioClip> deathSounds = new List<AudioClip>();
    
    public List<AudioClip> shootSounds = new List<AudioClip>();

    public PostDeathDummy postDeathDummy;

    public Scrollbar healthBar;

    private List<PlayerModifier> playerModifiers;

    protected Vector2 Direction
    {
        get => direction;
        set => direction = value;
    }

    protected virtual void Awake()
    {
        playerModifiers = new List<PlayerModifier>();
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
        
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; ++i)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    materialTextures.Add(renderers[i].material.mainTexture);
                    materialColors.Add(renderers[i].material.color);
                }
            }
        }
        ReceiveDamageEvent.AddListener(SignalizeDamage);
        DeathEvent.AddListener(StopSignalizingDamage);
        LevelsScript.EndLevelEvent.AddListener(delegate { Heal(healFrac * maxHealth); });

        retryBaseHealth = maxHealth;
        retryBaseAcceleration = acceleration;
        retryBaseAngularSpeed = angularSpeed;
        retryBaseMaxMovSpeed = maxMovSpeed;
        initialTransform = gameObject.transform;
        MenuEventBroker.Retry += OnRetry;
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
        ManageDamageDisplay();

        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        UpdateMovementSpeed();
        UpdateRotation();

        foreach (PlayerModifier pm in playerModifiers)
        {
            pm.update_effect();
        }
    }

    private void Heal(float amount)
    {
        if(affiliation == Affiliation.Player)
        {
            health += amount;
            if (health > maxHealth) health = maxHealth;
            MenuEventBroker.CallHealthChange(health / maxHealth);
        }
    }
    
    public virtual void ReceiveDamage(float amount)
    {
        if(invincibilityTime + lastDamage < Time.time)
        {
            lastDamage = Time.time;

            ReceiveDamageEvent.Invoke(amount);

            foreach (PlayerModifier pm in playerModifiers)
            {
                pm.damaged_effect();
            }

            if (health - amount <= 0f)
            {
                health = 0f;
                DeathEvent.Invoke();
                if (affiliation == Affiliation.Player) { MenuEventBroker.CallHealthChange(health / maxHealth); }
                if (affiliation == Affiliation.Player) { MenuEventBroker.CallPlayerKilled(); }
            }
            else
            {
                health -= amount;
                if (affiliation == Affiliation.Player) { MenuEventBroker.CallHealthChange(health / maxHealth); }
            }
        }
        
    }

    internal void AddModifier(PlayerModifier playerModifier)
    {
        bool exists = false;
        PlayerModifier existingOne = null;
        Type typ = playerModifier.GetType();
        foreach (PlayerModifier go in playerModifiers)
        {
            if (go.GetType() == typ)
            {
                exists = true;
                existingOne = go;
            }
        }
        if (exists)
            existingOne.strenght++;
        else
            playerModifiers.Add(playerModifier);
        playerModifier.normal_effect(this);
    }

    protected virtual void Die()
    {
        PostDeathDummy dummy = Instantiate(postDeathDummy);
        dummy.gameObject.transform.position = transform.position;
        PlayDeathSound(dummy);

        PlayDeathParticles(dummy);
        gameObject.SetActive(false);
        playerModifiers = new List<PlayerModifier>();
    }

    protected virtual void PlayDeathSound(PostDeathDummy dummy)
    {
        if (audioSource != null && deathSounds != null && deathSounds.Count > 0)
        {
            int audioIndex = Random.Range(0, deathSounds.Count);
            AudioClip audioClip = deathSounds[audioIndex];
            dummy.DeathSound = audioClip;
        }
    }

    protected virtual void PlayShootSound()
    {
        if (audioSource != null && shootSounds != null && shootSounds.Count > 0)
        {
            int audioIndex = Random.Range(0, shootSounds.Count);
            AudioClip audioClip = shootSounds[audioIndex];
            audioSource.PlayOneShot(audioClip);
        }
    }

    protected virtual void PlayDeathParticles(PostDeathDummy dummy)
    {
        if (dummy.deathParticlesPrefab != null)
        {
            GameObject particlesGameObject = Instantiate(dummy.deathParticlesPrefab);
            particlesGameObject.gameObject.transform.position = transform.position;
            var particleSystem = particlesGameObject.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(speed.x, 0f, speed.y);
    }

    private AudioSource audioSource = null;

    private float signalizationRemainingTime = 0f;

    private bool isSignalizingDamage = false;

    private List<Color> materialColors = new List<Color>();

    private List<Texture> materialTextures = new List<Texture>();

    protected void SignalizeDamage(float amount)
    {
        isSignalizingDamage = true;
        signalizationRemainingTime = damageSignalizationTime;

        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; ++i)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.color = damageColor;
                    renderers[i].material.mainTexture = damageTexture;
                }
            }
        }
    }

    protected void StopSignalizingDamage()
    {
        isSignalizingDamage = false;
        signalizationRemainingTime = 0f;

        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; ++i)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.color = materialColors[i];
                    renderers[i].material.mainTexture = materialTextures[i];
                }
            }
        }
    }

    protected void ManageDamageDisplay()
    {
        if (isSignalizingDamage)
        {
            if (signalizationRemainingTime > 0f)
            {
                signalizationRemainingTime -= Time.deltaTime;
            }
            else
            {
                StopSignalizingDamage();
            }
        }
    }

    protected virtual void OnRetry()
    {
        gameObject.SetActive(true);
        maxHealth = retryBaseHealth;
        health = retryBaseHealth;
        acceleration = retryBaseAcceleration;
        angularSpeed = retryBaseAngularSpeed;
        maxMovSpeed = retryBaseMaxMovSpeed;
        gameObject.transform.position = initialTransform.position;
        gameObject.transform.rotation = initialTransform.rotation;
        MenuEventBroker.CallHealthChange(health/maxHealth);
    }

    private float retryBaseHealth;
    private float retryBaseMaxMovSpeed, retryBaseAcceleration, retryBaseAngularSpeed;

    private Transform initialTransform;

    Vector3 ITarget.GetPosition()
    {
        return transform.position;
    }

    UnityEvent ITarget.GetDeathEvent()
    {
        return DeathEvent;
    }
}
