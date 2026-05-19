using UnityEngine;

public class LuminaGroveBootstrapper : MonoBehaviour
{
    void Start()
    {
        EnemySpawner.Instance.SpawnPatrolEnemy();
    }
}