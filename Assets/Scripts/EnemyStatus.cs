using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
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
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);//�ړ����x�ɉ����ăA�j���[�V������ω�������
    }
    private OnDieEvent OnEnemyDie = new OnDieEvent();

    public OnDieEvent EnewmyDieEvent => OnEnemyDie;

    [System.Serializable]
    public class OnDieEvent : UnityEvent<EnemyStatus> { }

    protected override void OnDie()
    {
        OnEnemyDie?.Invoke(this);
        StartCoroutine(DestroyCoroutine());
    }
    
}
