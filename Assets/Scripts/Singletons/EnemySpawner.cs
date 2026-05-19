using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [SerializeField] ChaseEnemy chaseEnemyPrefab;
    [SerializeField] PatrolEnemy patrolEnemyPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform[] patrolPoints;
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

    public void SpawnChaseEnemy()
    {
        if (chaseEnemyPrefab == null) return;
        Instantiate(chaseEnemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void SpawnPatrolEnemy()
    {
        if (patrolEnemyPrefab == null) return;

        PatrolEnemy enemy = Instantiate(patrolEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.SetPatrolPoints(patrolPoints);
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnChaseEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StopSpawning() => StopAllCoroutines();
    public void StartSpawning() => StartCoroutine(SpawnLoop());
}