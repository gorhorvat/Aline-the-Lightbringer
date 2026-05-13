using UnityEngine;

public class LuminaGroveBootstrapper : MonoBehaviour
{
    void Start()
    {
        EnemySpawner.Instance.SpawnChaseEnemy(Vector3.zero);
    }
}