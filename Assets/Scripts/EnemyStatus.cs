using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections;

public class EnemyStatus : CommonStatus
{

    private NavMeshAgent agent;
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }

    protected void Update()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude); // 移動の入力値をアニメーターに渡す
    }

    private OnDieEvent OnEnemyDie = new OnDieEvent();

    public OnDieEvent EnewmyDieEvent => OnEnemyDie;

    [System.Serializable]
    public class OnDieEvent : UnityEvent<EnemyStatus> { }

    protected override void OnDie()
    {
        OnEnemyDie?.Invoke(this);
        base.OnDie();
        //agent.isStopped = true;
        // StartCoroutine(DestroyCoroutine());
    }

    public override void GotoAttackStateIfPossible()
    {
        if (!IsAlive) return;
        //agent.isStopped = true;
        base.GotoAttackStateIfPossible();
        // 数秒後に移動を再開する
        StartCoroutine(ResumeMovementAfterDelay(1.0f));
    }

    private IEnumerator ResumeMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //agent.isStopped = false;
    }
}
