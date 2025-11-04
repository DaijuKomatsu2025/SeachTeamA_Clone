using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class SpawnerBoss : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private ParticleSystem _spawnEffect;
    [SerializeField] private AudioClip _spawnSound;
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Collider _collider;
    [SerializeField] private int _maxSpawnCount = 21;
    private float spawnDistance = 3f;
    private int _spawnCount = 0;
    private float rad = 0f;
    private Transform _target;
    private bool _isSpawning;

    [SerializeField] private TargetCountUIController _uiController;

    public List<EnemyStatus> SpawnedEnemies { get; set; } = new List<EnemyStatus>();

    private void Start()
    {
        _isSpawning = false;
    }

    public void SetUiController(TargetCountUIController uiController)
    {
        this._uiController = uiController;
        if (this._uiController != null)
        {
            Debug.Log("uiController attached");
        }
    }

    IEnumerator SpawnLoop()
    {
        if (_spawnEffect != null)
        {
            var eff = Instantiate(_spawnEffect, this.transform.position, Quaternion.identity);
            eff.transform.localScale = Vector3.one * 2f;
            eff.Play();
        }

        if (_spawnSound != null)
        {
            AudioSource.PlayClipAtPoint(_spawnSound, transform.position);
        }

        BossSpawn();

        yield return new WaitForSeconds(0.01f);

        while (true)
        {
            //Debug.Log("swawnCount:" + _spawnCount); 

            if(_isSpawning && SpawnedEnemies.Count == 0)
            {
                StartCoroutine(DestroyDeray());
                break;
            }
            else if (_spawnCount >= _maxSpawnCount)
            {
                yield return new WaitForSeconds(10f);
                continue;
            }
            else
            {
                if(spawnDistance % 10 == 0)
                {
                    spawnDistance += 2f;
                }
                var distanceVector = new Vector3(spawnDistance + Random.Range(-1.5f, 1.5f), 0);

                var spawnPositionFromAround = Quaternion.Euler(0, rad, 0) * distanceVector;

                rad += 30f;

                var spawnPosition = transform.position + spawnPositionFromAround;

                NavMeshHit hit;

                var enemyPrefabIndex = 1;// bat

                if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
                {
                    var rotation = Quaternion.Euler(0, Random.Range(0, 180), 0);

                    var enemy = Instantiate(_enemyPrefabs[enemyPrefabIndex], hit.position, rotation);
                    enemy.gameObject.name = _enemyPrefabs[enemyPrefabIndex].name + "_" + _spawnCount.ToString("00");
                    var enemyStatus = enemy.GetComponent<EnemyStatus>();
                    enemyStatus.gameObject.GetComponent<EnemyFollow_Bat>().player = _target;

                    enemyStatus.EnewmyDieEvent.AddListener(OnEnemyDefeated);

                    SpawnedEnemies.Add(enemyStatus);
                    _spawnCount++;

                    //UI更新
                    UpdateTargetUI();
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    private void BossSpawn()
    {
        var spawnPosition = transform.position;
        NavMeshHit hit;
        var enemyPrefabIndex = 0;// boss

        if (NavMesh.SamplePosition(spawnPosition, out hit, 10f, NavMesh.AllAreas))
        {
            var rotation = Quaternion.Euler(0, Random.Range(0, 180), 0);

            var enemy = Instantiate(_enemyPrefabs[enemyPrefabIndex], hit.position, rotation);
            enemy.gameObject.name = _enemyPrefabs[enemyPrefabIndex].name + "_" + _spawnCount.ToString("00");
            var enemyStatus = enemy.GetComponent<EnemyStatus>();
            enemyStatus.gameObject.GetComponent<EnemyFollow>().player = _target;

            enemyStatus.EnewmyDieEvent.AddListener(OnEnemyDefeated);

            SpawnedEnemies.Add(enemyStatus);
            _spawnCount++;
            _isSpawning = true;

            //UI更新
            UpdateTargetUI();
        }
    }

    IEnumerator BossCamera()
    {
        if (_camera != null)
        {
            _camera.Priority = 21;
            yield return new WaitForSeconds(0.6f);
            _camera.Priority = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isSpawning && other.gameObject.tag.Contains("Player"))
        {
            GameModeManager.SetGameMode(GameModeManager.GameMode.Annihilate);
            _target = other.transform;
            StartCoroutine(BossCamera());
            StartCoroutine(SpawnLoop());
        }
    }

    IEnumerator DestroyDeray()
    {
        yield return new WaitForSeconds(1f);
        GameModeManager.SetGameMode(GameModeManager.GameMode.Explore);
        Destroy(gameObject);
    }

    public void OnEnemyDefeated(EnemyStatus enemy)
    {
        if (SpawnedEnemies.Contains(enemy))
        {
            SpawnedEnemies.Remove(enemy);

            //UI更新
            UpdateTargetUI();
        }
    }

    public int GetRemainingEnemys() => SpawnedEnemies.Count;

    public int GetMaxEnemys() => _maxSpawnCount;


    /// <summary>
    /// 現在の敵の数に基づいてUIの表示を更新する
    /// </summary>
    private void UpdateTargetUI()
    {
        // リストのCountを使用して、現在の敵の残り数を取得
        int remainingCount = SpawnedEnemies.Count;

        // UIコントローラーに通知
        if (_uiController != null)
        {
            _uiController.UpdateTargetCount(remainingCount, _maxSpawnCount);
        }
    }
}
