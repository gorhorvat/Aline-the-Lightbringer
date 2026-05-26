public class ChronoFeatherRewardScreen : BaseRewardScreen
{
    public void Show(string levelName, float finalTime)
    {
        string message = $"Congratulations! Your final time: {Utils.FormatTime(finalTime)}";
        ShowScreen(message);
    }
}