using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [SerializeField] GameObject chaseEnemyPrefab;
    [SerializeField] GameObject patrolEnemyPrefab;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] bool autoSpawn = false;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (autoSpawn)
            StartCoroutine(SpawnLoop());
    }

    public void SpawnChaseEnemy(Vector3 position)
    {
        if (chaseEnemyPrefab == null) return;
        Instantiate(chaseEnemyPrefab, position, Quaternion.identity);
    }

    public void SpawnPatrolEnemy(Vector3 position, float rangeX, float rangeZ)
    {
        if (patrolEnemyPrefab == null) return;

        GameObject enemy = Instantiate(patrolEnemyPrefab, position, Quaternion.identity);

        // Pass patrol range so waypoints generate around spawn position
        if (enemy.TryGetComponent(out PatrolEnemy patrolEnemy))
        {
            patrolEnemy.SetPatrolRange(position, rangeX, rangeZ);
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnChaseEnemy(RandomPosition());
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(
            Random.Range(-10f, 10f),
            0f,
            Random.Range(-10f, 10f)
        );
    }

    public void StopSpawning() => StopAllCoroutines();
    public void StartSpawning() => StartCoroutine(SpawnLoop());
}