using UnityEngine;

public class ChaseEnemy : BaseEnemy
{
    Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").transform;
    }

    protected override Vector3 GetTargetPosition()
    {
        return player.position;
    }
}