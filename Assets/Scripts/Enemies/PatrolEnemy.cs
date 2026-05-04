using UnityEngine;

public class PatrolEnemy : BaseEnemy
{
    [SerializeField] float waypointTolerance = 0.5f;

    private Vector3[] waypoints;
    private int currentWaypoint;
    private Vector3 center;
    private float rangeX;
    private float rangeZ;

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

    private void GenerateWaypoints()
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