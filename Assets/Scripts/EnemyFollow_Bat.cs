using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow_Bat : MonoBehaviour
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
            //一定距離内に入ったら停止
            if (Vector3.Distance(transform.position, player.position) < 0.8f)
            {
                agent.isStopped = true; // プレイヤーに近づいたら停止
            }
            else
            {
                agent.isStopped = false; // プレイヤーから離れたら移動再開
            }
        }
        //死んだらNavMeshAgentを停止
        var status = GetComponent<CommonStatus>();

        if (status != null && !status.IsAlive)
        {

            agent.isStopped = true;
        }

    }
}
