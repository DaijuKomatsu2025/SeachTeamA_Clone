using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStatus : MobStatus
{
    private NavMeshAgent _agent;

    private OnDieEvent OnEnemyDie = new OnDieEvent();

    public OnDieEvent EnewmyDieEvent => OnEnemyDie;

    [System.Serializable]
    public class OnDieEvent : UnityEvent<EnemyStatus> { }

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        //if (_state is StateEnum.Normal)
        //{
        //    _animator.SetFloat("MoveSpeed", _agent.velocity.magnitude);
        //}
    }

    protected override void OnDie()
    {
        OnEnemyDie?.Invoke(this);
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnEnemyDie.RemoveAllListeners();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            _life = 0;
            _agent.isStopped = true;
            OnDie();
        }
    }
}
