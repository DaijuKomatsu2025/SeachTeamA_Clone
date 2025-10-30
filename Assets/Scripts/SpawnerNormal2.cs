using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class SpawnerNormal2 : MonoBehaviour
{
    private const int _spawnRadius = 1;
    private const float MaxDistance = 2f;
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Collider _collider;
    [SerializeField] private float _spawnDuration = 1.0f;
    [SerializeField] private int _maxSpawnCount = 20;
    private int _spawnCount = 0;
    private Transform _target;
    private bool _isSpawning;
    private Coroutine _spawnCoroutine;

    public List<EnemyStatus> SpawnedEnemies { get; set; } = new List<EnemyStatus>();

    private void Start()
    {
        _isSpawning = false;
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (_spawnCount >= _maxSpawnCount)
            {
                yield return new WaitForSeconds(_spawnDuration);
                continue;
            }
            else
            {
                var distanceVector = new Vector3(_spawnRadius, 0);
                var spawnPositionFromAround = Quaternion.Euler(0, Random.Range(0, 360), 0) * distanceVector;
                var spawnPosition = transform.position + spawnPositionFromAround;

                NavMeshHit hit;

                var enemyPrefabIndex = Random.Range(0, _enemyPrefabs.Length);

                if (NavMesh.SamplePosition(spawnPosition, out hit, MaxDistance, NavMesh.AllAreas))
                {
                    var rotation = Quaternion.Euler(0, Random.Range(0, 180), 0);

                    var enemy = Instantiate(_enemyPrefabs[enemyPrefabIndex], hit.position, rotation);
                    enemy.gameObject.name = _enemyPrefabs[enemyPrefabIndex].name + "_" + _spawnCount.ToString("00");
                    var enemyStatus = enemy.GetComponent<EnemyStatus>();
                    if (enemyStatus != null)
                    {
                        enemyStatus.gameObject.GetComponent<BossAttack>().player = _target;

                        enemyStatus.EnewmyDieEvent.AddListener(OnEnemyDefeated);
                        SpawnedEnemies.Add(enemyStatus);
                        _spawnCount++;
                        _isSpawning = true;
                    }
                }

                yield return new WaitForSeconds(_spawnDuration);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            if(!_isSpawning && SpawnedEnemies.Count == 0)
            {
                _target = other.transform;
                _spawnCoroutine = StartCoroutine(SpawnLoop());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            StopCoroutine(_spawnCoroutine);
        }
    }

    public void OnEnemyDefeated(EnemyStatus enemy)
    {
        if (SpawnedEnemies.Contains(enemy))
        {
            SpawnedEnemies.Remove(enemy);
            _spawnCount--;

            if (_spawnCount == 0) _isSpawning = false;
        }
    }
}
