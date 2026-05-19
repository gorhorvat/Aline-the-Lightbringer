using UnityEngine;
using TMPro;

public class CollectablesOverlay : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text lumiberryText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] GameObject beaconCrystalIcon;
    [SerializeField] GameObject chronoFeatherIcon;
    [SerializeField] GameObject radiantEmblemIcon;
    [SerializeField] GameObject ancientOwlRelicIcon;
    [SerializeField] GameObject starShardIcon;

    public void UpdateLumiberries(int current, int total)
    {
        lumiberryText.text = $"{current} ( {total} )";
        panel.SetActive(true);
    }

    public void UpdateLives(int lives)
    {
        livesText.text = $"{lives}";
    }

    public void UpdateCollectableIcons(CollectableType type, bool isActive)
    {
        switch (type)
        {
            case CollectableType.BeaconCrystal:
                beaconCrystalIcon.SetActive(isActive);
                break;
            case CollectableType.ChronoFeather:
                chronoFeatherIcon.SetActive(isActive);
                break;
            case CollectableType.RadiantEmblem:
                radiantEmblemIcon.SetActive(isActive);
                break;
            case CollectableType.AncientOwlRelic:
                ancientOwlRelicIcon.SetActive(isActive);
                break;
            case CollectableType.StarShard:
                starShardIcon.SetActive(isActive);
                break;
        }
    }

    public void Hide() => panel.SetActive(false);
}