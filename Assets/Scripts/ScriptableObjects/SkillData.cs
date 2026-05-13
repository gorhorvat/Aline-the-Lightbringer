using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game/SkillData")]
public class SkillData : ScriptableObject
{
    const int MaxUpgradeLevel = 3;
    const int InitiaUpgradeLevel = 0;
    
    public bool isUnlocked;
    public int upgradeLevel;

    public bool IsFullyUpgraded => upgradeLevel == MaxUpgradeLevel;

    public void Upgrade()
    {
        if (!IsFullyUpgraded)
            upgradeLevel++;
    }

    public void Reset()
    {
        isUnlocked = false;
        upgradeLevel = InitiaUpgradeLevel;
    }
}