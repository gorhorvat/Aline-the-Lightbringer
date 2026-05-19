using System;
using UnityEngine;

public class CollectablesIcons : MonoBehaviour
{
    [SerializeField] GameObject beaconCrystalIcon;
    [SerializeField] GameObject chronoFeatherIcon;
    [SerializeField] GameObject radiantEmblemIcon;
    [SerializeField] GameObject ancientOwlRelicIcon;
    [SerializeField] GameObject starShardIcon;

    public void UpdateIcon(CollectableType type, bool isActive)
    {
        switch (type)
        {
            case CollectableType.BeaconCrystal: beaconCrystalIcon.SetActive(isActive); break;
            case CollectableType.ChronoFeather: chronoFeatherIcon.SetActive(isActive); break;
            case CollectableType.RadiantEmblem: radiantEmblemIcon.SetActive(isActive); break;
            case CollectableType.AncientOwlRelic: ancientOwlRelicIcon.SetActive(isActive); break;
            case CollectableType.StarShard: starShardIcon.SetActive(isActive); break;
        }
    }

    public void RefreshFromLevelData(LevelData level)
    {
        foreach (CollectableType type in Enum.GetValues(typeof(CollectableType)))
            UpdateIcon(type, level != null && level.IsCollected(type));
    }
}