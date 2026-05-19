using UnityEngine;

public class PatrolEnemy : BaseEnemy
{
    Transform[] patrolPoints;
    int currentWaypoint;
    float waypointReachDistance = 0.5f;

    protected override Vector3 GetTargetPosition()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return transform.position;

        if (Vector3.Distance(transform.position, patrolPoints[currentWaypoint].position) < waypointReachDistance)
            currentWaypoint = (currentWaypoint + 1) % patrolPoints.Length;

        return patrolPoints[currentWaypoint].position;
    }

    public void SetPatrolPoints(Transform[] points)
    {
        patrolPoints = points;
    }
}