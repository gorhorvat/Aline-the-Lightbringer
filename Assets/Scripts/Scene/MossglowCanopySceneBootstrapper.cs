using UnityEngine;

public class MossglowCanopySceneBootstrapper : MonoBehaviour
{
    void Start()
    {
        EnemySpawner.Instance.SpawnPatrolEnemy();
    }
}