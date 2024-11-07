using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    private const int PoolCapacity = 10;
    private const int PoolMaxSize = 15;

    [SerializeField] private SpawnPoint[] _spawnPoints;
    [SerializeField] private Enemy _prefab;
    [SerializeField] private float _spawnDelay = 2.0f;

    private WaitForSeconds _spawnTime;
    private ObjectPool<Enemy> _enemyPool;

    private bool _isActive = true;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (enemy) => enemy.gameObject.SetActive(true),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy.gameObject),
            defaultCapacity: PoolCapacity,
            maxSize: PoolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnInterval());
    }

    public void StopSpawning()
    {
        _isActive = false;
    }

    private void Spawn()
    {
        Vector3 spawnPoint = GetSpawnPoint();
        Vector3 directionToMove = GetDirection();

        Enemy enemy = _enemyPool.Get();
        enemy.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
        enemy.SetDirection(directionToMove);
        enemy.Initialize(_enemyPool);
    }

    private Vector3 GetSpawnPoint()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
            return Vector3.zero;

        SpawnPoint spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        return spawnPoint.transform.position;
    }

    private Vector3 GetDirection()
    {
        return Vector3.right;
    }

    private IEnumerator SpawnInterval()
    {
        _spawnTime = new WaitForSeconds(_spawnDelay);

        while (_isActive)
        {
            yield return _spawnTime;
            Spawn();
        }
    }
}