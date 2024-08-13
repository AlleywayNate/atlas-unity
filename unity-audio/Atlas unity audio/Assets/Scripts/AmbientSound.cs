using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public AudioClip ambientClip; // Assign this in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Fix: GetComponent<AudioSource>()
        if (audioSource != null)
        {
            audioSource.clip = ambientClip;
            audioSource.spatialBlend = 1.0f; // Set to 3D
            audioSource.volume = 0.5f; // Initial volume
            audioSource.Play(); // Start playing the ambient sound
        }
    }
}
