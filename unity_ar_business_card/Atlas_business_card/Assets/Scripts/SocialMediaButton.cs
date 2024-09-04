using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SocialMediaButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Button button; // Attach the button component in the Inspector
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.gray;
    public Color pressedColor = Color.green;
    public AudioClip clickSound;
    public AudioClip hoverSound;
    private AudioSource audioSource;
    private Image buttonImage;

    void Start()
    {
        buttonImage = button.GetComponent<Image>();
        audioSource = gameObject.AddComponent<AudioSource>();
        buttonImage.color = defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = hoverColor;
        PlaySound(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = defaultColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.color = hoverColor;
        PlaySound(clickSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void OpenLinkedin()
    {
        Debug.Log("Opening LinkedIn...");
        Application.OpenURL("https://www.linkedin.com/in/nathenwilliams/");
    }

    public void OpenGithub()
    {
        Debug.Log("Opening Github...");
        Application.OpenURL("https://github.com/AlleywayNate");
    }

    public void OpenArtstation()
    {
        Debug.Log("Opening ArtStation...");
        Application.OpenURL("https://www.artstation.com/happilytwisted");
    }

    public void SendEmail()
    {
        Debug.Log("Sending email...");
        string email = "mailto:nathen.williams@atlasschool.com";
        Application.OpenURL(email);
    }
}
