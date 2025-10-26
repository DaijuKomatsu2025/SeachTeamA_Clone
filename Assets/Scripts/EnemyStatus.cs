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
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);//移動速度に応じてアニメーションを変化させる
    }
}
