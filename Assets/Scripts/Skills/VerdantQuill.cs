using UnityEngine;

public class VerdantQuill : BaseSkill
{
    public override void Execute()
    {
        Debug.Log($"Execute Verdant Quill skill level {skillData.upgradeLevel}");

        // base forest skill — always available
        //SpawnVine();

        // upgrade 1 — wider spread
        //if (HasUpgrade(1))
            //Skill2();

        // upgrade 2 — leaves burning ground
        //if (HasUpgrade(2))
            //Skill3();
    }
}
