using UnityEngine;
using UnityEngine.AI;

public class BossAttack : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Attack Settings")]
    public float detectRange = 15f;     // 攻撃を始める距離
    public float stopDistance = 10f;     // 追尾を止める距離
    public float attackCooldown = 1.5f;

    private float attackTimer = 0f;

    private NavMeshAgent agent;
    private Animator animator;
    private CommonStatus status;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        status = GetComponent<CommonStatus>();
    }

    void Update()
    {
        if (player == null || status == null || !status.IsAlive)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // プレイヤーが攻撃範囲外 → 追いかける
        if (distance > stopDistance && distance <= detectRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("Move", true);
        }
        // 一定範囲内 → 停止して攻撃
        else if (distance <= stopDistance)
        {
            agent.isStopped = true;

            // プレイヤーの方へ向く（NavMeshAgent の回転を利用）
            Vector3 look = (player.position - transform.position);
            look.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), 5f * Time.deltaTime);

            // 攻撃クールタイム
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                animator.SetTrigger("Attack");
                attackTimer = 0f;
            }
        }
    }

    // アニメーションイベントから呼ぶ
    void Shoot()
    {
        Vector3 dir = (player.position - firePoint.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);

        Instantiate(bulletPrefab, firePoint.position, rot);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}

