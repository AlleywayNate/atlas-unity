using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    private AudioSource bgmSource;
    public AudioClip victoryPianoClip; // Clip to play on victory
    private bool hasPlayedVictory = false; // Ensure victory music plays only once

    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        if (bgmSource != null)
        {
            bgmSource.Play(); // Start playing the BGM when the scene loads
        }
        else
        {
            Debug.LogWarning("BGM source is null");
        }
    }

    void Update()
    {
        // Stop the music if the player returns to the MainMenu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            StopBGM();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish") && !hasPlayedVictory)
        {
            HandleFinishFlag();
        }

        // Include any additional trigger logic you already have here
    }

    private void HandleFinishFlag()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop(); // Stop the current BGM
            Debug.Log("BGM stopped");
        }
        else
        {
            Debug.LogWarning("BGM source is null or not playing");
        }

        if (victoryPianoClip != null)
        {
            bgmSource.clip = victoryPianoClip; // Set the win music clip
            bgmSource.Play(); // Play the win music
            hasPlayedVictory = true; // Ensure it only plays once
        }
    }

    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop(); // Stop playing the BGM
        }
    }

    // Include any other methods or logic you already have here
    // For example:
    // private void AnotherMethod()
    // {
    //     // Your existing code here
    // }
}
