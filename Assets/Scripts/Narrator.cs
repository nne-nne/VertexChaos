using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Lore guy.
/// </summary>
public class Narrator : MonoBehaviour
{
    public List<AudioClip> remarks = new List<AudioClip>();

    public List<AudioClip> loreInfo = new List<AudioClip>();

    public void SayLore()
    {
        if (audioSource.isPlaying) { return; }
        
        if (currentLoreInfo.Current != null)
        {
            audioSource.PlayOneShot(currentLoreInfo.Current);
            currentLoreInfo.MoveNext();
        }
        else if (remarks != null)
        {
            audioSource.PlayOneShot(remarks[Random.Range(0, remarks.Count)]);
        }
    }

    private AudioSource audioSource;

    private IEnumerator<AudioClip> currentLoreInfo;

    private void Awake()
    {
        currentLoreInfo = loreInfo.GetEnumerator();
        currentLoreInfo.MoveNext();
        
        audioSource = gameObject.AddComponent<AudioSource>();

        LevelsScript.EndLevelEvent.AddListener(SayLore);
    }
}
