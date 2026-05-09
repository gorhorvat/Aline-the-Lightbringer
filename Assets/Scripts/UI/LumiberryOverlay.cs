using UnityEngine;
using TMPro;

public class LumiberryOverlay : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text lumiberryText;
    [SerializeField] TMP_Text livesText;

    public void UpdateLumiberries(int current, int total)
    {
        lumiberryText.text = $"{current} ( {total} )";
        panel.SetActive(true);
    }

    public void UpdateLives(int lives)
    {
        livesText.text = $"{lives}";
    }

    public void Hide() => panel.SetActive(false);
}