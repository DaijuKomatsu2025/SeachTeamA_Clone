using UnityEngine;
using TMPro;

// TargetCounterCanvas のルートGameObjectにアタッチすることを想定します。

public class TargetCountUIController : MonoBehaviour
{
    // TextMeshProコンポーネントへの参照（子オブジェクトに設定されていることを想定）
    [SerializeField]
    private TextMeshProUGUI targetCountText;

    // ターゲットの簡易アイコン（Image）への参照（任意）
    // [SerializeField] 
    // private UnityEngine.UI.Image targetIconImage; 

    private const string TEXT_FORMAT = "{1}/{0}";

    // 最大ターゲット数を保持する変数
    private int _maxTargets = 0;

    // Canvasのルートオブジェクトへの参照（このスクリプトがアタッチされているGameObject）
    private GameObject _rootCanvasObject;

    void Awake()
    {
        _rootCanvasObject = gameObject;

        // TMPコンポーネントが設定されているか確認
        //if (targetCountText == null)
        //{
        //    Debug.LogError("TargetCountText (TextMeshProUGUI) が設定されていません。インスペクターで設定してください。");
        //}

        // 初期状態では非表示にしておく
        _rootCanvasObject.SetActive(false);
    }

    /// <summary>
    /// 外部スクリプト（スポナー/イベントスクリプト）から呼び出され、ターゲット数を更新する
    /// </summary>
    /// <param name="count">現在のターゲットの残り数</param>
    public void UpdateTargetCount(int currentCount)
    {
        // ターゲット数が0より大きい場合（表示開始/更新）
        if (currentCount > 0)
        {
            // 最大数がまだ設定されていない場合は、現在のカウントを最大数として設定する
            // これは「最初に呼び出された時が最大数である」という前提
            if (_maxTargets == 0)
            {
                _maxTargets = currentCount;
            }

            // 1. Canvas全体を表示
            if (!_rootCanvasObject.activeSelf)
            {
                _rootCanvasObject.SetActive(true);
            }

            // 2. 残数と最大数をフォーマットして表示
            if (targetCountText != null)
            {
                // 💡 ここで残り数(currentCount)と最大数(_maxTargets)を渡す
                targetCountText.text = string.Format(TEXT_FORMAT, currentCount, _maxTargets);
            }
        }
        // ターゲット数が0以下の場合（表示終了）
        else
        {
            // ... (非表示処理は前回と同じ)
            if (_rootCanvasObject.activeSelf)
            {
                _rootCanvasObject.SetActive(false);
            }

            // 終了したら最大数をリセットしておくと次のイベントに対応しやすい
            _maxTargets = 0;
        }
    }
}