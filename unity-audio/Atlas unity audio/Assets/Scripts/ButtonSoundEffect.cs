using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private AudioClip hoverSound;  // Assign your hover sound clip in the Inspector
    private AudioClip clickSound;  // Assign your click sound clip in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Optional: Automatically load clips if not assigned in Inspector
        if (hoverSound == null)
        {
            hoverSound = Resources.Load<AudioClip>("Audio/SFX/UI/button-rollover");
        }
        if (clickSound == null)
        {
            clickSound = Resources.Load<AudioClip>("Audio/SFX/UI/button-click");
        }

        if (hoverSound == null || clickSound == null)
        {
            Debug.LogError("AudioClip(s) missing or not found.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}