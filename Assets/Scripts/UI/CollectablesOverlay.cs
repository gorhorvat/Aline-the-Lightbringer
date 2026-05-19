using UnityEngine;
using TMPro;

public class CollectablesOverlay : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text lumiberryText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] CollectablesIcons collectablesIcons;

    public void UpdateLumiberries(int current, int total)
    {
        lumiberryText.text = $"{current} ( {total} )";
        panel.SetActive(true);
    }

    public void UpdateLives(int lives)
    {
        livesText.text = $"{lives}";
    }

    public void UpdateCollectableIcon(CollectableType type, bool isActive)
    {
        collectablesIcons.UpdateIcon(type, isActive);
    }

    public void Hide() => panel.SetActive(false);
}