using UnityEngine;

public class VerticalMovingPlatform : BaseMovingPlatform
{
    protected override Vector3 GetTargetPosition()
    {
        return startPosition + Vector3.up * moveDistance;
    }
}