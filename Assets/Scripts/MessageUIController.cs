using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq; // GetSettingでLinqを使うため追加
using Unity.Cinemachine;

public class MessageUIController : MonoBehaviour
{
    // メッセージの種類を識別するための列挙型 (enum)
    public enum MessageType
    {
        EnterMonsterHouse, // モンスターハウス突入 (敵を全部倒せ！)
        PlayerDefeated,    // プレイヤーが全滅 (全滅)
        TimeUp             // 時間切れ (時間切れ)
    }

    [System.Serializable]
    public class MessageSetting
    {
        [Header("基本設定")]
        public MessageType type;
        public string messageText;           // 表示するテキスト (例: "敵を全部倒せ！")
        public TMP_FontAsset fontAsset;      // 使用するフォントアセット
        public float fontSize = 60;          // フォントサイズを指定

        [Header("グラデーションとテクスチャ設定(任意)")]
        // グラデーションの設定
        public Color topLeftColor = Color.white;
        public Color topRightColor = Color.white;
        public Color bottomLeftColor = Color.white;
        public Color bottomRightColor = Color.white;

        // テクスチャ（画像)設定
        public Texture mainTexture;
        [Header("サウンド設定(任意)")]
        // 効果音設定
        public AudioClip sfxClip;


        [Header("アニメーション設定")]
        public Animator animator;             // 使用するAnimatorコンポーネント
        public string animationTrigger;       // 再生するアニメーションのトリガー名
        public float animationDuration = 3.0f; // アニメーションの待ち時間 (コルーチン用)
    }

    [Header("UI要素への参照")]
    [SerializeField]
    private TextMeshProUGUI mainText; // メッセージを表示するTMPコンポーネント

    [Header("ゲーム制御要素への参照")]
    [SerializeField]
    private Timer gameTimerController; // 既存のTimer.csコンポーネントへの参照


    [Header("メッセージ設定リスト")]
    [SerializeField]
    private MessageSetting[] messageSettings; // メッセージごとの設定をリスト化

    private GameObject _rootCanvasObject; // メッセージUI（テキスト）のルートCanvasオブジェクトへの参照

    private AudioSource _audioSource; // 効果音再生用のAudioSource

    [Header("リスタートUIの設定")]
    // Inspectorから、ゲームオーバー時に表示するPanel/Canvasをアサイン
    [SerializeField] private GameObject _restartUIPanel;

    [SerializeField] private TextMeshProUGUI gameOverTimerText; // ゲームオーバー時に表示するタイマー用テキスト

    public AudioClip gameOverCountdownsfxClip; //ゲームオーバーカウントダウン専用SE再生用


    // 現在のシーン名（リスタート時に再ロードするために使用）
    private string _currentSceneName;

    // 🌟 ゲームオーバー処理の二重起動を防止するためのフラグ
    private bool _isGameOverProcessing = false;

    [Header("制御対象の他システム")]
    // 🌟 [修正] Timer.csへの参照（gameTimerController）に統一するため、_existingTimerフィールドを削除
    [SerializeField]
    private CinemachineBrain _cinemachineBrain; // 非アクティブにするCinemachineBrainコンポーネント
    [SerializeField]
    private GameObject _compassObject; // 非アクティブにするコンパス

    void Awake()
    {
        _rootCanvasObject = gameObject;
        if (mainText == null)
        {
            Debug.LogError("Main Text (TextMeshProUGUI) が設定されていません。");
        }

        // AudioSourceコンポーネントを自動で取得・追加
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 現在のシーン名を記録
        _currentSceneName = SceneManager.GetActiveScene().name;

        // 初期状態ではリスタートUIとメッセージUIを非表示にしておく
        _rootCanvasObject.SetActive(false);
        if (_restartUIPanel != null)
        {
            _restartUIPanel.SetActive(false);
        }
    }

    // ----------------------------------------------------
    // メッセージ表示処理（ゲームオーバーではないメッセージ用）
    // ----------------------------------------------------

    /// <summary>
    /// 外部からメッセージの種類を受け取って表示をトリガーする（非ゲームオーバーメッセージ用）
    /// </summary>
    public void ShowMessage(MessageType type)
    {
        // EnterMonsterHouseなどの非ゲームオーバーメッセージを想定
        if (type == MessageType.PlayerDefeated || type == MessageType.TimeUp)
        {
            Debug.LogWarning($"MessageType {type} はゲームオーバー処理です。ShowGameOverUI() を使用してください。");
            ShowGameOverUI(type); // 念のためゲームオーバー処理に回す
            return;
        }

        MessageSetting setting = GetSetting(type);

        if (setting == null)
        {
            Debug.LogWarning($"MessageType {type} の設定が見つかりません。");
            return;
        }

        // Canvasを表示して設定とアニメーションを開始
        _rootCanvasObject.SetActive(true);
        // 🌟 [修正] 非ゲームオーバーメッセージでも、テキストコンポーネントをアクティブにする
        mainText.gameObject.SetActive(true);
        ApplyMessageSetting(setting); // 共通メソッドで設定を適用

        // アニメーション終了後に自動でメッセージを非表示にするコルーチンを開始
        StartCoroutine(HideMessageAfterDelay(setting.animationDuration));
    }

    /// <summary>
    /// アニメーション時間経過後にメッセージUI全体を非表示にする
    /// </summary>
    private IEnumerator HideMessageAfterDelay(float delay)
    {
        // 🌟 [修正] Time.timeScale=0f の影響を受けないよう、WaitForSecondsRealtimeを使用
        yield return new WaitForSecondsRealtime(delay);
        // 🌟 [修正] ルートCanvas(gameObject)ではなく、テキストコンポーネントを非表示にする
        mainText.gameObject.SetActive(false);

        //// メッセージが乗っているルートCanvas全体を非表示にする
        //_rootCanvasObject.SetActive(false);
    }

    // ----------------------------------------------------
    // ゲームオーバー処理（全滅・時間切れ時のみ外部から呼び出す）
    // ----------------------------------------------------

    /// <summary>
    /// 全滅・時間切れ時に、メッセージ表示後にカウントダウンとシーンロードを行う
    /// </summary>
    /// <param name="type">PlayerDefeated または TimeUp</param>
    public void ShowGameOverUI(MessageType type)
    {
        // 🌟 [修正] 二重起動防止フラグをチェック
        if (_isGameOverProcessing) return;

        // 🌟 [修正] コルーチン起動 *前* にフラグを立てる
        _isGameOverProcessing = true;

        // 🌟 [修正] Timer.csの参照（gameTimerController）を非表示にする
        if (gameTimerController != null)
        {
            // TimerコンポーネントがアタッチされているGameObjectを取得し、非表示にする
            gameTimerController.gameObject.SetActive(false);
        }

        _rootCanvasObject.SetActive(true);

        // 参照の強制再取得（以前の修正）
        if (_restartUIPanel != null && gameOverTimerText == null)
        {
            gameOverTimerText = _restartUIPanel.GetComponentInChildren<TextMeshProUGUI>(true);
            if (gameOverTimerText == null)
            {
                Debug.LogError("リスタートパネルの子要素にGameOverTimerText(TMP)が見つかりません。");
            }
        }

        StartCoroutine(ShowMessageAndStartTimer(type));
    }

    /// <summary>
    /// ゲームオーバーメッセージ表示、アニメーション待ち、時間停止、リスタートタイマー開始を行うコルーチン
    /// </summary>
    private IEnumerator ShowMessageAndStartTimer(MessageType type)
    {
        // PlayerDefeated または TimeUp の設定のみを受け入れる
        if (type != MessageType.PlayerDefeated && type != MessageType.TimeUp)
        {
            Debug.LogError($"ShowGameOverUI に無効な MessageType ({type}) が渡されました。");
            _isGameOverProcessing = false; // フラグを戻す
            yield break;
        }

        MessageSetting setting = GetSetting(type);
        if (setting == null)
        {
            Debug.LogError($"GameOver MessageType {type} の設定が見つかりません。");
            _isGameOverProcessing = false; // フラグを戻す
            yield break;
        }

        // 1. メッセージアニメーションの実行
        _rootCanvasObject.SetActive(true);

        // 🌟 [修正] テキストコンポーネントをアクティブにする
        mainText.gameObject.SetActive(true);

        ApplyMessageSetting(setting); // 共通メソッドで設定を適用

        // 🌟 [修正] Time.timeScale=0f の影響を受けないよう、WaitForSecondsRealtimeを使用
        yield return new WaitForSecondsRealtime(setting.animationDuration);

        // 🌟 [修正] ルートCanvas(gameObject)ではなく、mainTextコンポーネントを非表示にする
        mainText.gameObject.SetActive(false);

        //// 2. メッセージを非表示/廃棄 (ルートCanvasを非表示にする)
        //_rootCanvasObject.SetActive(false);

        // 🌟 [修正] Time.timeScale=0f の *前* に、他のシステムを停止する
        // (gameTimerControllerはShowGameOverUIで停止済み)

        // 🌟 Cinemachineのコンポーネントだけを無効化🌟
        if (_cinemachineBrain != null)
        {
            _cinemachineBrain.enabled = false; // GameObjectを止めずにコンポーネントを止める
        }
        if (_compassObject != null)
        {
            _compassObject.SetActive(false);
        }

        // 3. アニメーション完了後に時間を停止
        Time.timeScale = 0f;

        // 4. リスタートパネルを表示（ボタンとタイマーを含む）
        if (_restartUIPanel != null)
        {
            _restartUIPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("リスタートUIパネルが設定されていません。");
            _isGameOverProcessing = false; // フラグを戻す
            yield break;
        }

        // 5. ゲームオーバーカウントダウンを開始 (15秒)
        int timer = 15;
        if (gameOverTimerText != null)// タイマー用テキストが設定されている場合
            gameOverTimerText.text = timer.ToString("00"); // 🌟 [修正] Mathf.Ceil不要、intを直接表示

        // int currentSecond = Mathf.CeilToInt(timer); // 🌟 [修正] 冗長なため削除
        int previousSecond = timer; // 🌟 [修正] 最初の秒数を設定

        while (timer > 0)
        {
            yield return new WaitForSecondsRealtime(1.0f);// 1秒待機

            timer--; // Time.timeScale=0f のため、UnscaledDeltaTimeを使用

            int currentSecond = timer; // 🌟 [修正] timerの値をそのまま使用

            // タイマー用テキストを更新
            if (gameOverTimerText != null)
            {
                gameOverTimerText.text = currentSecond.ToString("00");
            }

            // 秒数が変わった瞬間のみSEを再生
            if (currentSecond < previousSecond)
            {
                // 🌟 [修正] 0秒になった時は鳴らさない (timer > 0 のチェックをループ末尾に移動)
                if (timer > 0 && gameOverCountdownsfxClip != null && _audioSource != null)
                {
                    _audioSource.PlayOneShot(gameOverCountdownsfxClip);
                }
                previousSecond = currentSecond; // 秒数を更新
            }
        }

        // 6. カウントダウン終了後、ゲームオーバーシーンをロード
        Time.timeScale = 1f; // ロード前に時間を戻す
        SceneManager.LoadScene("GameOverScene");
    }

    // ----------------------------------------------------
    // 共通ユーティリティ
    // ----------------------------------------------------

    /// <summary>
    /// MessageSettingをTextMeshProに適用する共通処理
    /// </summary>
    private void ApplyMessageSetting(MessageSetting setting)
    {
        mainText.text = setting.messageText;
        if (setting.fontAsset != null)
        {
            mainText.font = setting.fontAsset;
        }
        mainText.fontSize = setting.fontSize;

        // グラデーションの設定を適用
        mainText.enableVertexGradient = true;

        // 🌟 [修正] グラデーションのインスタンス生成を簡略化（元のコードでも動作はする）
        TMP_ColorGradient gradient = new TMP_ColorGradient(
            setting.topLeftColor,
            setting.topRightColor,
            setting.bottomLeftColor,
            setting.bottomRightColor
        );
        mainText.colorGradientPreset = gradient;

        // テクスチャをマテリアルに適用
        if (setting.mainTexture != null)
        {
            // 🌟 [修正] マテリアルのインスタンス化を避けるため SharedMaterial を使用
            // （ただし、複数のTMPが同じマテリアルを共有すると問題が起きる場合は .material を使う）
            Material mat = mainText.fontSharedMaterial;
            if (mat != null)
            {
                // 🌟 [修正] ShaderUtilities.ID_MainTex を使用（TMProの標準的な方法）
                mat.SetTexture(ShaderUtilities.ID_MainTex, setting.mainTexture);
            }
        }

        // サウンド再生
        if (setting.sfxClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(setting.sfxClip);
        }

        // アニメーションを再生
        if (setting.animator != null && !string.IsNullOrEmpty(setting.animationTrigger))
        {
            setting.animator.SetTrigger(setting.animationTrigger);
        }
    }


    /// <summary>
    /// メッセージ設定リストから該当する設定を取得する
    /// </summary>
    private MessageSetting GetSetting(MessageType type)
    {
        // Linqを使用してリストから設定を検索
        return messageSettings.FirstOrDefault(s => s.type == type);
    }

    // ----------------------------------------------------
    // アニメーターイベント/リスタートボタンからの呼び出し
    // ----------------------------------------------------

    /// <summary>
    /// リスタートボタンのOnClickに設定するメソッド
    /// </summary>
    public void RestartGame()
    {
        // 1. 停止した時間を再開
        Time.timeScale = 1f;

        // 2. UIを非表示
        if (_restartUIPanel != null)
        {
            _restartUIPanel.SetActive(false);
        }

        // 🌟 [修正] 停止したシステムを復元
        if (gameTimerController != null)
        {
            gameTimerController.gameObject.SetActive(true);
        }
        if (_cinemachineBrain != null)
        {
            _cinemachineBrain.enabled = true;
        }
        if (_compassObject != null)
        {
            _compassObject.SetActive(true);
        }

        GameModeManager.currentMode = GameModeManager.GameMode.Explore;// モードを探索モードにリセット

        // 3. 現在のシーンを再ロードしてリスタート
        SceneManager.LoadScene(_currentSceneName);
    }
}

