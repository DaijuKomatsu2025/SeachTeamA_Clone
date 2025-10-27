using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CommonStatus))]
public class CommonAttack : MonoBehaviour
{
    [SerializeField] private float attackInterval = 1.0f;//攻撃間隔
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
        AttackIfPossible();
    }
    public void OnAttackStart()
    {
        attackcollider.enabled = true;
    }
    public void OnAttackHit(Collider collider)
    {
        //攻撃判定に触れたオブジェクトにダメージを与える
        var targetStatus = collider.GetComponent<CommonStatus>();
        if (null== targetStatus) return;
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
}
