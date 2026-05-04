using UnityEngine;

public class HorizontalMovingPlatform : BaseMovingPlatform
{
    protected override Vector3 GetTargetPosition()
    {
        return startPosition + Vector3.right * moveDistance;
    }
}