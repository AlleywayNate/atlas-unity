using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; // Required for Audio Mixer
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Play state
    public static bool isPaused = false;

    public GameObject PauseCanvas;
    public AudioMixer AudioMixer; // Reference to your Audio Mixer
    public string normalSnapshotName = "NormalSnapshot";
    public string muffledSnapshotName = "MuffledSnapshot";

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            PauseCanvas.SetActive(true);
            AudioMixer.FindSnapshot(muffledSnapshotName).TransitionTo(0.1f); // Muffle BGM
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
            PauseCanvas.SetActive(false);
            AudioMixer.FindSnapshot(normalSnapshotName).TransitionTo(0.1f); // Restore BGM
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Options()
    {
        SceneManager.LoadScene(1);
    }
}
