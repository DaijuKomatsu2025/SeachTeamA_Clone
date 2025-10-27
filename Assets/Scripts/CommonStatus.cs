using UnityEngine;

public class CommonStatus : MonoBehaviour

{
    protected enum StateEnum
    {
        Normal,
        Attacking,
        Dead
    }
    [SerializeField] private int hp;//体力
    [SerializeField] private int maxHp;//最大体力
    [SerializeField] public int attack;//攻撃力
    [SerializeField] private float moveSpeed;//移動速度
    [SerializeField] public float MoveSpeed { get { return moveSpeed; } }
    public bool IsMovable => StateEnum.Normal == state;//移動可能かどうか

    public bool IsAttackable => StateEnum.Normal == state;//攻撃可能かどうか
    protected StateEnum state = StateEnum.Normal;//状態
    
    [SerializeField] protected Animator animator;//アニメーター
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void OnDie()//死んだときの処理
    {
        state = StateEnum.Dead;
        animator.SetTrigger("Die");
        //死んだら数秒後消える
        Destroy(gameObject, 3.0f);

    }
    public void ReturnToNormalState()
    {
        if (state == StateEnum.Dead) return;
        state = StateEnum.Normal;
    }
    public void GotoAttackStateIfPossible()
    {
        if (!IsAttackable) return;
        state = StateEnum.Attacking;
        animator.SetTrigger("Attack");



    }

    public void Damage(int attack)
    {
        if (state == StateEnum.Dead) return;
        hp -= attack;
        if (hp > 0) return;
        OnDie();
    }
    public int GetMaxHp()
    {
        return maxHp;
    }
    public int GetHp()
    {
        return hp;
    }



}
