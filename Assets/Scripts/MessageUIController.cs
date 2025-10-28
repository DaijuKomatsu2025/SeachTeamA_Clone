using UnityEngine;
using TMPro;

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
        public MessageType type;
        public string messageText;             // 表示するテキスト (例: "敵を全部倒せ！")
        public TMP_FontAsset fontAsset;        // 使用するフォントアセット (筆調 or 既存)
        public float fontSize = 60;            //フォントサイズを指定
        public Animator animator;              // 使用するAnimatorコンポーネント
        public string animationTrigger;        // 再生するアニメーションのトリガー名
    }

    [Header("UI要素への参照")]
    [SerializeField]
    private TextMeshProUGUI mainText; // メッセージを表示するTMPコンポーネメント

    [Header("メッセージ設定リスト")]
    [SerializeField]
    private MessageSetting[] messageSettings; // メッセージごとの設定をリスト化

    private GameObject _rootCanvasObject;

    void Awake()
    {
        _rootCanvasObject = gameObject;
        if (mainText == null)
        {
            Debug.LogError("Main Text (TextMeshProUGUI) が設定されていません。");
        }
        _rootCanvasObject.SetActive(false); // 初期非表示
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

        // 1. Canvasを表示
        _rootCanvasObject.SetActive(true);

        // 2. テキストとフォントを設定
        mainText.text = setting.messageText;
        if (setting.fontAsset != null)
        {
            mainText.font = setting.fontAsset;
        }

        // 3. アニメーションを再生
        if (setting.animator != null && !string.IsNullOrEmpty(setting.animationTrigger))
        {
            setting.animator.SetTrigger(setting.animationTrigger);
        }
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
}