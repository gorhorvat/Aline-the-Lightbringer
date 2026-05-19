using UnityEngine;

public class DiagonalPlatform : BaseMovingPlatform
{
    [SerializeField] float diagonalX = 1f;

    protected override Vector3 GetTargetPosition()
    {
        return startPosition + new Vector3(reverse ? -diagonalX : diagonalX, 1f, 0f).normalized * moveDistance;
    }
}