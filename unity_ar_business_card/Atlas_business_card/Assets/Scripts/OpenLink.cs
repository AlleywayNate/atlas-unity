using UnityEngine;
using UnityEngine.UI;

public class OpenLink : MonoBehaviour
{
    public Button button;
    public string url;

    void Start()
    {
        button.onClick.AddListener(OpenURL);
    }

    void OpenURL()
    {
        Application.OpenURL(url);
    }
}