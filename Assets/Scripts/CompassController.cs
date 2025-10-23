using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class CompassController : MonoBehaviour
{
    // 【プレイヤーのTransformをインスペクターから明示的に設定
    [SerializeField]
    private Transform player;

    // TargetのTransform
    [SerializeField]
    private Transform target;

    // コンパスの針はこのスクリプトがアタッチされたRectTransformを使用
    private RectTransform compassVisual;

    // コンパスの画像が右向きの場合など、初期角度のズレを修正するオフセット
    [SerializeField]
    private float initialAngleOffset = 90f; // 例: 画像が右向きなら90度

    // 【追加】リングの色制御用のフィールド
    [Header("--- Ring Color Settings ---")]
    [Tooltip("色を変えるリングのImageコンポーネメント")]
    [SerializeField] private Image ringImage; // ★インスペクターから設定
    [SerializeField] private float maxColorDistance;// 色が変わりきる最大距離
    [SerializeField] private Color nearColor;// 近距離の色
    [SerializeField] private Color farColor;// 遠距離の色


    void Awake()
    {
        // スクリプトがアタッチされたオブジェクトのRectTransformを取得
        compassVisual = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        // プレイヤーまたはターゲットが設定されていない場合は処理を中断
        if (player == null || target == null || compassVisual == null)
        {
            Debug.LogWarning("PlayerまたはTargetがインスペクターに設定されていません。");
            return;
        }


        // 1. プレイヤーからTargetへの方向ベクトルを計算
        // 明示的に指定された2つのTransformの位置を参照します。
        Vector3 directionToTarget = target.position - player.position;

        // Y軸を無視した水平方向のベクトルを抽出
        Vector3 flatDirection = Vector3.ProjectOnPlane(directionToTarget, Vector3.up).normalized;

        float distanceToTarget = directionToTarget.magnitude;// ターゲットまでの距離を計算(3D距離)

        // 2. 角度を計算 (プレイヤーの正面 (Z軸) からTargetへの角度)
        float angle = Vector3.SignedAngle(Vector3.forward, flatDirection, Vector3.up);

        // 3. UIのZ軸回転値として角度を設定（オフセット適用と符号反転）
        // 符号を反転させることで、3D空間の角度をUIのZ軸回転に合わせる
        float finalZRotation = -(angle + initialAngleOffset);

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, finalZRotation);

        // 4. 回転を適用（スムーズさが必要ならSlerpを使用）
        compassVisual.localRotation = targetRotation;

        // 【追加】リングの色を距離に基づいて変更
        if (ringImage != null)
        {
            // 1. 【最終修正】Near Color と Far Color の Alpha 値を強制的に不透明に設定
            nearColor.a = 1f; // 完全に不透明 (100%)
            farColor.a = 1f;  // 完全に不透明 (100%)

            // 2. 距離の正規化
            float normalizedDistance = Mathf.Clamp01(distanceToTarget / maxColorDistance);

            // 3. 色の線形補間を適用
            // Alphaは1fに固定されているので、RGBだけがLerpされる
            ringImage.color = Color.Lerp(nearColor, farColor, normalizedDistance);
        }

        // 【追加】リングの色を距離に基づいて変更
        if (ringImage != null)
        {
            float normalizedDistance = Mathf.Clamp01(distanceToTarget / maxColorDistance);
            ringImage.color = Color.Lerp(nearColor, farColor, normalizedDistance);
        }

    }
}