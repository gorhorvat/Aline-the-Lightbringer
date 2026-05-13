using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HubData", menuName = "Game/HubData")]
public class HubData : ScriptableObject
{
    public List<ZoneData> zones;

    public ZoneData GetZoneData(string zoneName) => zones.Find(z => z.name == zoneName);

    public LevelData GetLevelData(string levelName)
    {
        foreach (ZoneData zone in zones)
        {
            LevelData level = zone.GetLevelData(levelName);
            if (level != null) return level;
        }
        return null;
    }

    public bool IsZoneUnlocked(ZoneData zone) => zone.isUnlocked;

    public void Reset()
    {
        foreach (ZoneData zone in zones) zone.Reset();
    }
}