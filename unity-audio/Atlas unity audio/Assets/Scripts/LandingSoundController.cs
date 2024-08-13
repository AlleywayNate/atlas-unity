using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingSoundController : MonoBehaviour
{
    public AudioClip landingGrassClip;
    public AudioClip landingRockClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.y < -1) // Check if falling
        {
            if (collision.collider.CompareTag("Grass"))
            {
                PlayLandingSound(landingGrassClip);
            }
            else if (collision.collider.CompareTag("Rock"))
            {
                PlayLandingSound(landingRockClip);
            }
        }
    }

    private void PlayLandingSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
