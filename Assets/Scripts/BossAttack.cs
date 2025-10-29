using UnityEngine;


public class BossAttack : MonoBehaviour
{
    [Header("References")]
    public Transform player;         // プレイヤー
    public Transform firePoint;      // 発射位置（口）
    public GameObject bulletPrefab;  // 弾のPrefab

    [Header("Attack Settings")]
    public float detectRange = 15f;  // プレイヤーを感知する範囲
    public float attackCooldown = 2f;
    public float rotateSpeed = 3f;   // 向きを変えるスピード

    private float attackTimer = 0f;

    [SerializeField] private Animator animator;

    void Update()
    {
        if (player == null || firePoint == null || bulletPrefab == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 一定距離内なら攻撃
        if (distance <= detectRange)
        {
            // 🔹 プレイヤーの方向を向く（Y軸だけ回転）
            Vector3 targetDir = (player.position - transform.position);
            targetDir.y = 0f; // 上下の角度を無視
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
            animator.SetTrigger("Attack");

            // 🔹 攻撃タイマー
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                Shoot();
                attackTimer = 0f;
            }
        }
        else
        {
            // 範囲外では攻撃タイマーをリセット
            attackTimer = 0f;
        }

    }

    void Shoot()
    {
        // プレイヤー方向を基準に弾を生成
        Vector3 direction = (player.position - firePoint.position).normalized;
        Quaternion rot = Quaternion.LookRotation(direction);
        
        Instantiate(bulletPrefab, firePoint.position, rot);
        Debug.Log("💥 Boss fires at player!");
    }

    // シーン上で攻撃範囲を可視化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
