using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    [SerializeField] protected SkillData skillData;

    public abstract void Execute();

    protected bool HasUpgrade(int level) => skillData.upgradeLevel >= level;
}