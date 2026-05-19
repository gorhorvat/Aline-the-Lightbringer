using UnityEngine;

public class LuminaGroveBootstrapper : MonoBehaviour
{
    void Start()
    {
        EnemySpawner.Instance.SpawnPatrolEnemy(Vector3.zero, 10, 10);
    }
}