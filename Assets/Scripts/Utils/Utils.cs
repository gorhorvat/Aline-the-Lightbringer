using UnityEngine;

public static class Utils
{
    public static bool IsBelowKillPlane(Vector3 position, float killY = -20f)
    {
        return position.y < killY;
    }

    public static string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time * 1000) % 1000);
        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}