using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    public bool isUnlocked;

    public bool hasBeaconCrystal = true;
    public bool beaconCrystalCollected;

    public bool hasChronoFeather = true;
    public float chronoFeatherTargetTime;
    public float chronoFeatherBestTime;
    public bool chronoFeatherCollected;
    
    public bool hasRadiantEmblem = true;
    public bool radiantEmblemCollected;
    
    public bool hasAncientOwlRelic;
    public bool ancientOwlRelicCollected;
    
    public bool hasStarShard;
    public bool starShardCollected;

    public bool IsCollected(CollectableType type)
    {
        return type switch
        {
            CollectableType.BeaconCrystal => hasBeaconCrystal && beaconCrystalCollected,
            CollectableType.ChronoFeather => hasChronoFeather && chronoFeatherCollected,
            CollectableType.RadiantEmblem => hasRadiantEmblem && radiantEmblemCollected,
            CollectableType.AncientOwlRelic => hasAncientOwlRelic && ancientOwlRelicCollected,
            CollectableType.StarShard => hasStarShard && starShardCollected,
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
        chronoFeatherBestTime = 0f;
        radiantEmblemCollected = false;
        ancientOwlRelicCollected = false;
        starShardCollected = false;
    }
}