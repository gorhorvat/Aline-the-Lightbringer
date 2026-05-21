using UnityEngine;

public class RadiantEmblemRewardScreen : BaseRewardScreen
{
    public void Show(string levelName, string targetLevel)
    {
        string message = $"Congratulations for being deathless in {Levels.GetDisplayValue(levelName)}!";
        ShowScreen(message, targetLevel);
    }
}