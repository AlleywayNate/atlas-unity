using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public Slider BGMSlider; // Reference to the BGM slider
    private AudioSource bgmAudioSource; // Reference to the BGM AudioSource

    // Initialize and load settings
    void Start() 
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // Find the BGM AudioSource (assuming it's on a GameObject named "BGM")
        bgmAudioSource = GameObject.Find("BGM").GetComponent<AudioSource>();

        // Load saved volume settings
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        BGMSlider.value = savedVolume;
        UpdateVolume(savedVolume);
    }

    // Update BGM volume based on slider value
    public void OnBGMSliderValueChanged(float value)
    {
        UpdateVolume(value);
        PlayerPrefs.SetFloat("BGMVolume", value); // Save the volume setting
    }

    private void UpdateVolume(float sliderValue)
    {
        float volume = ConvertDecibelsToVolume(sliderValue);
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
        }
    }

    // Convert decibel number to audio source volume number (0 to 1)
    private float ConvertDecibelsToVolume(float dB)
    {
        return Mathf.Clamp01(Mathf.Pow(10, dB / 20));
    }

    // Load previous scene (kinda)
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
