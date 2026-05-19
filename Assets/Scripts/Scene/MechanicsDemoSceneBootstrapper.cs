using UnityEngine;

public class MechanicsDemoSceneBootstrapper : MonoBehaviour
{
    void Start()
    {
        // Spawn initial enemies at specific locations
        EnemySpawner.Instance.SpawnPatrolEnemy();
        EnemySpawner.Instance.SpawnChaseEnemy();
    }
}