using UnityEngine;

public static class Utils
{
    public static bool IsBelowKillPlane(Vector3 position, float killY = -20f)
    {
        return position.y < killY;
    }
}