using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class SpawnerAnnihilate : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Collider _collider;
    [SerializeField] private int _maxSpawnCount = 20;
    private int _spawnCount = 0;
    private Transform _target;
    private bool _isSpawning;

    private TargetCountUIController _uiController;

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
        yield return new WaitForSeconds(3f);

        while (true)
        {
            Debug.Log("swawnCount:" + _spawnCount); 

            if(_isSpawning && SpawnedEnemies.Count == 0)
            {
                StartCoroutine(DestroyDeray());
                break;
            }
            else if (_spawnCount == _maxSpawnCount)
            {
                yield return new WaitForSeconds(10f);
                continue;
            }
            else
            {
                var distanceVector = new Vector3(1, 0);
                var spawnPositionFromAround = Quaternion.Euler(0, Random.Range(0, 360), 0) * distanceVector;
                var spawnPosition = transform.position + spawnPositionFromAround;

                NavMeshHit hit;

                var enemyPrefabIndex = Random.Range(0, _enemyPrefabs.Length);

                if (NavMesh.SamplePosition(spawnPosition, out hit, 10f, NavMesh.AllAreas))
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

                _isSpawning = true;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            GameModeManager.SetGameMode(GameModeManager.GameMode.Annihilate);
            _target = other.transform;
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
            _uiController.UpdateTargetCount(remainingCount);
        }
    }

}
