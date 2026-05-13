using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZoneData", menuName = "Game/ZoneData")]
public class ZoneData : ScriptableObject
{
    public List<LevelData> levels;

    public bool isUnlocked;
    public bool bossDefeated;
    public bool obeliskActivated;
    public bool newSkillCollected;

    public LevelData GetLevelData(string levelName) => levels.Find(l => l.name == levelName);

    public void Reset()
    {
        bossDefeated = false;
        obeliskActivated = false;
        newSkillCollected = false;

        foreach (LevelData level in levels) level.Reset();
    }
}