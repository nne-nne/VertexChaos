using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

/// <summary>
/// Class to display effects after controller is deactivated.
/// </summary>
public class PostDeathDummy : MonoBehaviour
{
    public AudioClip DeathSound
    {
        get => deathSound;
        set { deathSound = value; audioSource.PlayOneShot(deathSound); }
    }
    
    public GameObject deathParticlesPrefab = null;
    
    public float timeToDeactivate = 5f;

    public void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponentInParent<PlayerController>();
        if (controller != null && controller.affiliation != Affiliation.Enemy)
        {
            controller.ReceiveDamage(explosionDamage);
        }
    }

    private AudioClip deathSound;

    private AudioSource audioSource;

    public float explosionDamage = 0;
    

    private void FixedUpdate()
    {
        if (timeToDeactivate > 0)
        {
            timeToDeactivate -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}