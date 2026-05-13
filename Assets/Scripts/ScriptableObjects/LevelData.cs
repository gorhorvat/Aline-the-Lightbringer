using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    public bool isUnlocked;
    public bool beaconCrystalCollected;
    public bool chronoFeatherCollected;
    public bool radiantEmblemCollected;
    public bool ancientOwlRelicCollected;
    public bool starShardCollected;

    public bool IsCollected(CollectableType type)
    {
        return type switch
        {
            CollectableType.BeaconCrystal => beaconCrystalCollected,
            CollectableType.ChronoFeather => chronoFeatherCollected,
            CollectableType.RadiantEmblem => radiantEmblemCollected,
            CollectableType.AncientOwlRelic => ancientOwlRelicCollected,
            CollectableType.StarShard => starShardCollected,
            _ => false
        };
    }

    public void Collect(CollectableType type)
    {
        switch (type)
        {
            case CollectableType.BeaconCrystal: beaconCrystalCollected = true; break;
            case CollectableType.ChronoFeather: chronoFeatherCollected = true; break;
            case CollectableType.RadiantEmblem: radiantEmblemCollected = true; break;
            case CollectableType.AncientOwlRelic: ancientOwlRelicCollected = true; break;
            case CollectableType.StarShard: starShardCollected = true; break;
        }
    }

    public void Reset()
    {
        beaconCrystalCollected = false;
        chronoFeatherCollected = false;
        radiantEmblemCollected = false;
        ancientOwlRelicCollected = false;
        starShardCollected = false;
    }
}