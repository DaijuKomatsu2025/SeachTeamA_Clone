using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(CommonStatus))]
public class CommonAttack : MonoBehaviour
{
    [SerializeField] private float attackInterval = 2.0f;//攻撃間隔
    [SerializeField] private Collider attackcollider;//攻撃判定用コライダー
    private CommonStatus status;
    private void Start()
    {
        status = GetComponent<CommonStatus>();
    }

    private void AttackIfPossible()
    {
        if (!status.IsAttackable) return;
        status.GotoAttackStateIfPossible();
    }
    public void OnAttackRangeEnter(Collider other)
    {
        if (!status.IsAlive) return;
        StartCoroutine(AttackDelay(1.5f));
       
    }
    public void OnAttackStart()
    {
        attackcollider.enabled = true;
    }
    public void OnAttackHit(Collider collider)
    {
        //攻撃判定に触れたオブジェクトにダメージを与える
        var targetStatus = collider.GetComponent<CommonStatus>();
        if (null == targetStatus) return;
        Debug.Log("Attack Hit!");
        targetStatus.Damage(status.attack);

    }
    public void OnAttackEnd()
    {
        attackcollider.enabled = false;
        //一定時間後に再度攻撃可能にする
        StartCoroutine(ResetAttackableAfterInterval());
    }
    private IEnumerator ResetAttackableAfterInterval()
    {
        yield return new WaitForSeconds(attackInterval);
        status.ReturnToNormalState();
    }
    private IEnumerator AttackDelay(float delay) 
    {
        yield return new WaitForSeconds(delay);
        AttackIfPossible();
    }
}
