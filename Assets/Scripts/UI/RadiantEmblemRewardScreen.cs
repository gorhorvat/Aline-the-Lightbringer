using UnityEngine;

public class RadiantEmblemRewardScreen : BaseRewardScreen
{
    public void Show(string levelName)
    {
        string message = $"Congratulations for being deathless in {Levels.GetDisplayValue(levelName)}!";
        ShowScreen(message);
    }
}