using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    const int InitialLives = 5;

    public int currentLives;
    public int currentLumiberries;
    public int totalLumiberries;

    // hub
    public HubData hub;

    // skills
    public List<SkillData> skills;

    public SkillData equippedSkill;

    public SkillData GetSkillData(string skillName) => skills.Find(s => s.name == skillName);

    void OnEnable() => Reset();

    public void Reset()
    {
        currentLives = InitialLives;
        currentLumiberries = 0;
        totalLumiberries = 0;
        equippedSkill = null;

        hub.Reset();

        foreach (SkillData skill in skills) skill.Reset();
    }
}