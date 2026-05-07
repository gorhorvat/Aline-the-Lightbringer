using UnityEngine;

public class PatrolEnemy : BaseEnemy
{
    float waypointTolerance = 0.5f;

    Vector3[] waypoints;
    int currentWaypoint;
    Vector3 center;
    float rangeX;
    float rangeZ;

    protected override void Awake()
    {
        base.Awake();
        GenerateWaypoints();
    }

    public void SetPatrolRange(Vector3 center, float rangeX, float rangeZ)
    {
        this.center = center;
        this.rangeX = rangeX;
        this.rangeZ = rangeZ;
        GenerateWaypoints();
    }

    void GenerateWaypoints()
    {
        waypoints = new Vector3[4];
        waypoints[0] = new Vector3(center.x - rangeX, transform.position.y, center.z - rangeZ);
        waypoints[1] = new Vector3(center.x + rangeX, transform.position.y, center.z - rangeZ);
        waypoints[2] = new Vector3(center.x + rangeX, transform.position.y, center.z + rangeZ);
        waypoints[3] = new Vector3(center.x - rangeX, transform.position.y, center.z + rangeZ);
    }

    protected override Vector3 GetTargetPosition()
    {
        if (waypoints.Length == 0) return transform.position;

        Vector3 target = waypoints[currentWaypoint];

        if (Vector3.Distance(transform.position, target) < waypointTolerance)
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;

        return target;
    }
}