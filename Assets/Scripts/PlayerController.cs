using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CommonAttack))]

public class PlayerController : CommonStatus
{
    [SerializeField] private float waitForStopCamera = 3f;

    //[SerializeField]
    //private float MoveSpeed;//移動速度
    //[SerializeField]
    //private Animator animator;//アニメーターコンポーネント
    private CharacterController characterController;// キャラクターコントローラーコンポーネント
    private Transform _transform;// プレイヤーのTransformコンポーネント
    private Vector3 moveVelocity;//移動速度ベクトル
    private InputAction move;//移動入力アクション
    private InputAction _attack;//攻撃入力アクション
    public float StopSpeed = 0.01f;//停止速度
    public float StopTime = 0;//停止時間
    private bool isStopping = false;//停止中かどうか
    private Vector3 LastPostion;//最後の位置
    [SerializeField]
    private CinemachineCamera StopCamera;

    //タイマーにアクセスするための参照
    private Timer _timer;

    private MessageUIController _messageController;// メッセージUIコントローラーへの参照


    protected override void Start()
    {
        base.Start();
        characterController = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
        var inputActionAsset = GetComponent<PlayerInput>().actions;
        move = inputActionAsset.FindAction("Move");//移動アクションを取得
        _attack = inputActionAsset.FindAction("Attack");//攻撃アクションを取得
        LastPostion = transform.position;//初期位置を設定

        _messageController = FindFirstObjectByType<MessageUIController>(FindObjectsInactive.Include);

        //Timer クラスの参照を取得
        _timer = FindFirstObjectByType<Timer>();

        //MessageUIController の参照を取得
        _messageController = FindFirstObjectByType<MessageUIController>(FindObjectsInactive.Include);

    }

    void Update()
    {
        if (!characterController.enabled) return;
        var inputVector = move.ReadValue<Vector2>();//移動入力ベクトルを取得
        moveVelocity = new Vector3(inputVector.x, 0, inputVector.y);//移動速度ベクトルを設定
        characterController.Move(moveVelocity * MoveSpeed * Time.deltaTime);//キャラクターを移動

        if (_attack.triggered)//攻撃入力があった場合
        {
            GotoAttackStateIfPossible();//攻撃状態に移行
        }
        //移動方向に向ける
        transform.LookAt(_transform.position + new Vector3(moveVelocity.x, 0, moveVelocity.z));

        //重力処理
        moveVelocity.y += Physics.gravity.y * Time.deltaTime;

        //アニメーション処理
        animator.SetFloat("MoveSpeed", new Vector3(moveVelocity.x, 0, moveVelocity.z).magnitude);

        float distance = Vector3.Distance(LastPostion, transform.position);
        if (distance < StopSpeed)//停止している場合
        {
            isStopping = true;
            StopTime += Time.deltaTime;
            if (StopTime >= waitForStopCamera)
            {
                StopCamera.Priority = 10;//カメラの優先度を上げる
            }
            //else
            //{
            //    StopCamera.Priority = 9;//カメラの優先度を元に戻す
            //}
        }
        else//移動している場合
        {
            if(isStopping) StopCamera.Priority = 9;//カメラの優先度を元に戻す
            isStopping = false;
            StopTime = 0;
        }
        LastPostion = transform.position;//最後の位置を更新

        if (state == StateEnum.Dead)
        {
            characterController.enabled = false;//キャラクターコントローラーを無効化

            if (_timer != null)
            {
                // StopAndResetTimer(true) を呼び出し、停止とリセットを実行
                _timer.StopAndResetTimer(true);
            }


            if (_messageController != null)//メッセージUIコントローラーを確認して全滅メッセージを入れる
            {
                // MessageType.PlayerDefeated を指定して呼び出す
                _messageController.ShowMessage(MessageUIController.MessageType.PlayerDefeated);

                //// UIコントローラーに「ゲームオーバー状態である」ことを通知
                ////    これにより、UI側がアニメーション終了後にボタンを表示する準備が整う
                //_messageController.SetGameOverState(MessageUIController.MessageType.PlayerDefeated);
            }

        }
    }
}
