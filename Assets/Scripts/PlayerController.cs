using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed;//移動速度
    //[SerializeField]
    //private float RunSpeed;// 走る速度追加式
    private CharacterController characterController;// キャラクターコントローラーコンポーネント
    private Transform transform;// プレイヤーのTransformコンポーネント
    private Vector3 moveVelocity;//移動速度ベクトル
    private InputAction move;//移動入力アクション
    //private InputAction sprint; // 走る入力アクション追加式



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        var inputActionAsset = GetComponent<PlayerInput>().actions;
        move = inputActionAsset.FindAction("Move");//移動アクションを取得
        //sprint = inputActionAsset.FindAction("Sprint"); // 走るアクションを取得追加式

    }

    void Update()
    {
        var inputVector = move.ReadValue<Vector2>();//移動入力ベクトルを取得
        moveVelocity = new Vector3(inputVector.x, 0, inputVector.y);//移動速度ベクトルを設定
        characterController.Move(moveVelocity * MoveSpeed * Time.deltaTime);//キャラクターを移動
        //移動方向に向ける
        transform.LookAt(transform.position + new Vector3(moveVelocity.x, 0, moveVelocity.z));

        
        //重力処理
        moveVelocity.y += Physics.gravity.y * Time.deltaTime;//Y方向の速度に重力を加算
    }
}
