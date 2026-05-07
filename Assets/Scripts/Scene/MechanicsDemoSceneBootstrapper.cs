using UnityEngine;

public class MechanicsDemoSceneBootstrapper : MonoBehaviour
{
    void Start()
    {
        // Spawn initial enemies at specific locations
        EnemySpawner.Instance.SpawnPatrolEnemy(new Vector3(0f, 0f, 0f), 5f, 5f);
        EnemySpawner.Instance.SpawnChaseEnemy(new Vector3(-5f, 0f, -5f));
    }
}