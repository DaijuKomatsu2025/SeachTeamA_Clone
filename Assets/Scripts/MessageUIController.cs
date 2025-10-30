using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public string messageText;             // 表示するテキスト (例: "敵を全部倒せ！")
        public TMP_FontAsset fontAsset;        // 使用するフォントアセット (筆調 or 既存)
        public float fontSize = 60;            //フォントサイズを指定

        [Header("グラデーションとテクスチャ設定(任意)")]
        //グラデーションの設定
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
        public Animator animator;              // 使用するAnimatorコンポーネント
        public string animationTrigger;        // 再生するアニメーションのトリガー名
    }

    [Header("UI要素への参照")]
    [SerializeField]
    private TextMeshProUGUI mainText; // メッセージを表示するTMPコンポーネメント

    [Header("メッセージ設定リスト")]
    [SerializeField]
    private MessageSetting[] messageSettings; // メッセージごとの設定をリスト化

    private GameObject _rootCanvasObject;// ルートCanvasオブジェクトへの参照

    private AudioSource _audioSource;// 効果音再生用のAudioSource

    [Header("リスタートUIの設定")]
    //Inspectorから、ゲームオーバー時に表示するPanel/Canvasをアサイン
    [SerializeField] private GameObject _restartUIPanel;

    // 現在のシーン名（リスタート時に再ロードするために使用）
    private string _currentSceneName;

    private MessageType _pendingGameOverType;


    void Awake()
    {
        _rootCanvasObject = gameObject;
        if (mainText == null)
        {
            Debug.LogError("Main Text (TextMeshProUGUI) が設定されていません。");
        }
        _rootCanvasObject.SetActive(false); // 初期非表示

        // AudioSourceコンポーネントを自動で取得・追加
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 現在のシーン名を記録
        _currentSceneName = SceneManager.GetActiveScene().name;

        //初期状態ではリスタートUIを非表示にしておく
        if (_restartUIPanel != null)
        {
            _restartUIPanel.SetActive(false);
        }

    }
    /// <summary>
    /// 全滅・時間切れ時にゲームオーバー処理を実行し、リスタートUIを表示する
    /// </summary>
    public void GameOverAndShowRestart()
    {
        //スタートUIを表示
        if (_restartUIPanel != null)
        {
            _restartUIPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 外部からメッセージの種類を受け取って表示をトリガーする
    /// </summary>
    public void ShowMessage(MessageType type)
    {
        MessageSetting setting = GetSetting(type);

        if (setting == null)
        {
            Debug.LogWarning($"MessageType {type} の設定が見つかりません。");
            return;
        }

        // Canvasを表示
        _rootCanvasObject.SetActive(true);

        // テキストとフォントを設定
        mainText.text = setting.messageText;
        if (setting.fontAsset != null)
        {
            mainText.font = setting.fontAsset;
        }


        // グラデーションの設定を適用
        mainText.enableVertexGradient = true; // グラデーションを有効化

        TMP_ColorGradient gradient = ScriptableObject.CreateInstance<TMP_ColorGradient>();

        gradient.topLeft = setting.topLeftColor;
        gradient.topRight = setting.topRightColor;
        gradient.bottomLeft = setting.bottomLeftColor;
        gradient.bottomRight = setting.bottomRightColor;

        mainText.colorGradientPreset = gradient; // 新しいグラデーションを適用

        // テクスチャをマテリアルに適用
        if (setting.mainTexture != null)
        {
            // TextMeshProのレンダラーのマテリアルをコピーし、テクスチャをセット
            Material mat = mainText.fontMaterial;
            if (mat != null)
            {
                // "_MainTex" は通常、TextMeshProシェーダーが使用するテクスチャプロパティ名
                mat.SetTexture("_MainTex", setting.mainTexture);
            }
        }

        if (setting.sfxClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(setting.sfxClip);
        }

        // アニメーションを再生
        if (setting.animator != null && !string.IsNullOrEmpty(setting.animationTrigger))
        {
            setting.animator.SetTrigger(setting.animationTrigger);
        }
        if (type == MessageUIController.MessageType.TimeUp || type == MessageUIController.MessageType.PlayerDefeated)
        {
            GameOverAndShowRestart();
        }
        //OnMessageAnimationFinished();

    }

    /// <summary>
    /// アニメーション終了後に外部からこのメソッドを呼び出す想定
    /// </summary>
    public void HideMessage()
    {
        _rootCanvasObject.SetActive(false);
    }

    private MessageSetting GetSetting(MessageType type)
    {
        foreach (var setting in messageSettings)
        {
            if (setting.type == type)
            {
                return setting;
            }
        }
        return null;
    }
    public void RestartGame()
    {
        // 1. 停止していた時間を再開（最重要：フリーズ解除）
        Time.timeScale = 1f;

        // 2. リスタートUIパネルを非表示
        if (_restartUIPanel != null)
        {
            _restartUIPanel.SetActive(false);
        }

        // 3. 現在のシーンを再ロード（リスタート処理）
        SceneManager.LoadScene(_currentSceneName);
    }
    public void SetGameOverState(MessageType type)
    {
        // 全滅または時間切れの時のみフラグを立てる
        if (type == MessageType.PlayerDefeated || type == MessageType.TimeUp)
        {
            _pendingGameOverType = type;
        }
    }
    public void OnMessageAnimationFinished()
    {
        // 記憶しているメッセージタイプがゲームオーバーの種類であればボタンを表示する
        if (_pendingGameOverType == MessageType.PlayerDefeated || _pendingGameOverType == MessageType.TimeUp)
        {
            // 1. リスタートUIパネルを表示
            if (_restartUIPanel != null)
            {
                _restartUIPanel.SetActive(true);
            }

            // 2. 次のゲームオーバー処理のためにフラグをリセット
            //_pendingGameOverType = (MessageType)(-1); // 無効な値にリセット
        }

        //// アニメーションが終わったらメッセージテキストを非表示にする処理など...
        //HideMessage();
    }
}