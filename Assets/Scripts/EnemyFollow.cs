using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position); // プレイヤーの位置に向かって移動
        }
    }
}
