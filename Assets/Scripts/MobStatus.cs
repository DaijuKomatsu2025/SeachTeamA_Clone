using UnityEngine;

public abstract class MobStatus : MonoBehaviour
{
    public enum StateEnum
    {
        Normal,
        Attack,
        Die
    }

    public StateEnum _state = StateEnum.Normal;

    public bool IsMovable => _state == StateEnum.Normal;
    public bool IsAttackable => _state == StateEnum.Normal;

    public StateEnum GetState() => _state;

    [SerializeField] private float _lifeMax = 1;

    public float LifeMax => _lifeMax;

    protected float _life;

    public float Life => _life;

    protected Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        _life = _lifeMax;
    }

    protected virtual void OnDie()
    {
        _animator.SetTrigger("Die");
    }

    public virtual void TakeDamage(int damage, Vector3 forceDirection)
    {
        if (_state == StateEnum.Die) return;

        _life -= damage;

        if (_life > 0) return;

        _state = StateEnum.Die;
        OnDie();
    }

    public void GoToAttackIfPossible()
    {
        if (_state != StateEnum.Normal) return;

        _state = StateEnum.Attack;
        _animator.SetTrigger("Attack");
    }

    public void GoToNormalStateIfPossible()
    {
        if (_state is StateEnum.Die) return;
        _state = StateEnum.Normal;
    }

    protected virtual void OnDestroy() { }
}
