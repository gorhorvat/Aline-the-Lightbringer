using UnityEngine;

public class HorizontalMovingPlatform : BaseMovingPlatform
{
    protected override Vector3 GetTargetPosition()
    {
        return startPosition + (reverse ? Vector3.left : Vector3.right) * moveDistance;
    }
}