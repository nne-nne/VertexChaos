using System;
using System.Collections.Generic;
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
    
    private AudioClip deathSound;

    private AudioSource audioSource;

    private float timeToDeactivate = 60f;

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