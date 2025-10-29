using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPGaugeController : MonoBehaviour
{
    [Header("アニメーターからのHP情報引用元")]
    [Tooltip("CharacterIconAnimatorからPlayerStatusへの参照を流用します。")]
    public CharacterIconAnimator iconAnimator; // CharacterIconAnimatorへの参照

    [Header("HPゲージUI要素")]
    public Image hpGaugeImage;                  // ゲージのImageコンポーネント
    public TextMeshProUGUI hpText;              // HPの数値表示テキスト

    private CommonStatus _playerStatus;          // HP情報の本体
    private int _lastKnownHP = -1;              // 前回のHP値を記録

    void Start()
    {
        //if (iconAnimator == null)
        //{
        //    Debug.LogError("CharacterIconAnimator の参照が設定されていません。インスペクターで設定してください。");
        //    enabled = false;
        //    return;
        //}

        // 💡 CharacterIconAnimatorからCommonStatus（HP情報源）を間接的に取得
        _playerStatus = iconAnimator.playerStatus;

        //if (_playerStatus == null)
        //{
        //    Debug.LogError("CommonStatus (HP情報源) が見つかりません。");
        //    enabled = false;
        //}

        // 初期表示を更新
        UpdateHPDisplay();
    }

    void Update()
    {
        // HPの現在値を取得
        int currentHP = _playerStatus.GetHp();

        // HPが変化した場合のみ表示を更新
        if (currentHP != _lastKnownHP)
        {
            UpdateHPDisplay();
            _lastKnownHP = currentHP;
        }
    }

    /// <summary>
    /// HPゲージの増減とテキストの表示を同時に更新する
    /// </summary>
    private void UpdateHPDisplay()
    {
        int currentHP = _playerStatus.GetHp();
        int maxHP = _playerStatus.GetMaxHp();

        // 1. ゲージの Fill Amount を更新 (0.0 ～ 1.0)
        if (hpGaugeImage != null && maxHP > 0)
        {
            float fillAmount = (float)currentHP / maxHP;
            hpGaugeImage.fillAmount = fillAmount;
        }

        // 2. テキスト表示を更新 (例: "50/50")
        if (hpText != null)
        {
            hpText.text = $"HP:{currentHP}/{maxHP}";
        }
    }
}