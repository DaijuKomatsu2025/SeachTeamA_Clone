using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterIconAnimator : MonoBehaviour
{
    // ⭐ 修正ポイント 1: PlayerStatus（MobStatus）への参照
    [Header("HP引用元")]
    public CommonStatus playerStatus; // InspectorでPlayerStatusコンポーネントを持つオブジェクトを紐付けます

    // HPが衰弱状態に切り替わる閾値（40%）
    private const int WORN_THRESHOLD = 40;

    [Header("UIコンポーネント")]
    public Image iconImage;

    // (省略: テクスチャのSprite変数群は変更なし)
    [Header("健康時の目のテクスチャ")]
    public Sprite healthy_OpenEyes;
    public Sprite healthy_ClosedEyes;
    public Sprite healthy_HalfClosedEyes;

    [Header("衰弱時の目のテクスチャ")]
    public Sprite worn_OpenEyes;
    public Sprite worn_ClosedEyes;
    public Sprite worn_HalfClosedEyes;

    [Header("瀕死時のテクスチャ (HP 0)")]
    public Sprite dead_Fixed;

    [Header("目パチ設定")]
    public float blinkIntervalMin = 2.0f;
    public float blinkIntervalMax = 5.0f;
    public float blinkDuration = 0.1f;

    // 目パチ用のコルーチン
    private Coroutine blinkCoroutine;
    // ⭐ 修正ポイント 2: 前回のHP割合を記録
    private int lastKnownHPPercentage = -1;

    //PlayerRespawn用
    private CharacterIconAnimator _iconAnimator;

    void Start()
    {
        if (iconImage == null)
        {
            iconImage = GetComponent<Image>();
        }

        if (playerStatus == null)
        {
            Debug.LogError("Mob Status (PlayerStatus) が設定されていません。アイコンアニメーションは機能しません。");
            return;
        }

        // 初期状態でアイコンの状態を決定
        UpdateIconState(GetHPPercentage());
    }

    // ⭐ 修正ポイント 3: UpdateでHPを監視
    void Update()
    {
        //  HPが0以下かどうかチェックする
        if (playerStatus != null && playerStatus.GetHp() <= 0)
        {
            //HPが0以下なら、グラフィックを瀕死アイコンに固定する
            iconImage.sprite = dead_Fixed;

            //  実行中の目パチコルーチンを確実に停止
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }

            //このスクリプトを完全停止
            enabled = false;
            // これ以降の UpdateIconState() の呼び出しや、
            //      その他の更新処理を完全にスキップする（早期リターン）
            return;
        }

        int currentHPPercentage = GetHPPercentage();
        bool isBlinking = (blinkCoroutine != null); // 現在目パチコルーチンが動いているか

        // 1. HP割合が変化した場合の処理
        if (currentHPPercentage != lastKnownHPPercentage)
        {
            // HPが変化したら、すぐにアイコンの状態を更新し、目パチを再開（または瀕死で停止）する
            UpdateIconState(currentHPPercentage);
            lastKnownHPPercentage = currentHPPercentage;
        }
        // 2. HPは変化していないが、目パチが何らかの理由で停止している場合
        //    かつ、瀕死状態ではない場合（健康または衰弱）
        else if (!isBlinking && currentHPPercentage > 0)
        {
            // 目パチコルーチンが動いていないなら、現在のHPで再開する
            UpdateIconState(currentHPPercentage);
        }
    }

    /// <summary>
    /// MobStatusから現在のHP割合(0-100)を取得するヘルパー関数
    /// </summary>
    private int GetHPPercentage()
    {
        if (playerStatus == null || playerStatus.GetMaxHp() <= 0) return 100; // 安全対策

        // (現在HP / 最大HP) * 100 で割合を計算し、切り上げ整数で取得
        float percentage = ((float)playerStatus.GetHp() / (float)playerStatus.GetMaxHp()) * 100f;
        return Mathf.FloorToInt(percentage);
    }

    /// <summary>
    /// 現在のHP割合に基づき、アイコンの状態を更新する
    /// </summary>
    public void UpdateIconState(int currentHPPercentage)
    {
        // 実行中の目パチコルーチンを停止
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        // HP 0 の場合（瀕死状態）
        if (currentHPPercentage <= 0)
        {
            iconImage.sprite = dead_Fixed;
            return; // 瀕死時は目パチアニメーションは実行しない
        }
        // HP 100% ～ 26% の場合（健康状態）
        else if (currentHPPercentage > WORN_THRESHOLD) // WORN_THRESHOLD は 25
        {
            // 健康時の画像を設定し、目パチを開始
            iconImage.sprite = healthy_OpenEyes;
            // 共通のBlinkAnimationコルーチンを健康時の画像で実行
            blinkCoroutine = StartCoroutine(BlinkAnimation(
                healthy_OpenEyes, healthy_HalfClosedEyes, healthy_ClosedEyes));
        }
        // HP 25% ～ 1% の場合（衰弱状態）
        else
        {
            // 衰弱時の画像を設定し、目パチを開始
            iconImage.sprite = worn_OpenEyes;
            // 共通のBlinkAnimationコルーチンを衰弱時の画像で実行
            blinkCoroutine = StartCoroutine(BlinkAnimation(
                worn_OpenEyes, worn_HalfClosedEyes, worn_ClosedEyes));
        }
    }

    /// <summary>
    /// 共通の目パチアニメーションロジック (この部分は変更なし)
    /// </summary>
    private IEnumerator BlinkAnimation(Sprite open, Sprite halfClosed, Sprite closed)
    {
        iconImage.sprite = open;

        while (true)
        {
            float waitTime = Random.Range(blinkIntervalMin, blinkIntervalMax);
            yield return new WaitForSeconds(waitTime);

            // 👁️ 閉じるアニメーション
            iconImage.sprite = halfClosed;
            yield return new WaitForSeconds(blinkDuration / 3f);
            iconImage.sprite = closed;
            yield return new WaitForSeconds(blinkDuration / 3f);
            iconImage.sprite = halfClosed;
            yield return new WaitForSeconds(blinkDuration / 3f);

            // 開き目に戻す
            iconImage.sprite = open;
        }
    }
    public void PlayerRespawn()
    {
        // 1. HPを最大値に戻す
        if (playerStatus != null)
        {
            // GetHp(int) → hp フィールドを直接代入
            playerStatus.hp = playerStatus.GetMaxHp();
        }

        // 2. アイコンアニメーターの動作を再開させる
        if (_iconAnimator != null)
        {
            _iconAnimator.enabled = true;

            //アイコンの状態更新を強制的に実行
            //これにより、UpdateIconState() 内で目パチコルーチンが再起動される
            _iconAnimator.UpdateIconState(100); // HP 100% の状態を通知
        }
    }
}