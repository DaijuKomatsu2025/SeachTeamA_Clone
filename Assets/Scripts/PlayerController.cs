using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CommonAttack))]


public class PlayerController : CommonStatus
{
    //[SerializeField]
    //private float MoveSpeed;//移動速度
    //[SerializeField]
    //private Animator animator;//アニメーターコンポーネント
    private CharacterController characterController;// キャラクターコントローラーコンポーネント
    private Transform transform;// プレイヤーのTransformコンポーネント
    private Vector3 moveVelocity;//移動速度ベクトル
    private InputAction move;//移動入力アクション
    private InputAction _attack;//攻撃入力アクション



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        var inputActionAsset = GetComponent<PlayerInput>().actions;
        move = inputActionAsset.FindAction("Move");//移動アクションを取得
        _attack = inputActionAsset.FindAction("Attack");//攻撃アクションを取得
        //sprint = inputActionAsset.FindAction("Sprint"); // 走るアクションを取得追加式

    }

    void Update()
    {
        var inputVector = move.ReadValue<Vector2>();//移動入力ベクトルを取得
        moveVelocity = new Vector3(inputVector.x, 0, inputVector.y);//移動速度ベクトルを設定
        characterController.Move(moveVelocity * MoveSpeed * Time.deltaTime);//キャラクターを移動
        if (_attack.triggered)//攻撃入力があった場合
        {
            GotoAttackStateIfPossible();//攻撃状態に移行
        }
        //移動方向に向ける
        transform.LookAt(transform.position + new Vector3(moveVelocity.x, 0, moveVelocity.z));

        //重力処理
        moveVelocity.y += Physics.gravity.y * Time.deltaTime;

        //アニメーション処理
        animator.SetFloat("MoveSpeed", new Vector3(moveVelocity.x, 0, moveVelocity.z).magnitude);

    }
}
