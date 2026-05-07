using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text loadingText;

    public void Show(string message)
    {
        loadingText.text = message;
        panel.SetActive(true);
    }
    public void Hide() => panel.SetActive(false);
}