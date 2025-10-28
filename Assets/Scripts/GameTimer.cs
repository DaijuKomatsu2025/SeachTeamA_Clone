using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;//コルーチン用
using Random = UnityEngine.Random;//テキストメッシュプロを使う場合
public class Timer : MonoBehaviour
{
    // UIの場合 (Canvas上にあるTextMesh Pro)
    public TextMeshProUGUI tmpText;

    // TextMeshProUGUIコンポーネントをインスペクターからアタッチします
    public TextMeshProUGUI timeText;

    // 現在の秒数を保持する変数（例として600秒）
    public float totalTime = 600f;

    //★特定の時間でタイマーを動かす為のプロパティ★
    [Header("シェイク設定")]
    // シェイクの強さ（大きくすると激しく揺れる）
    public float shakeMagnitude = 10f;
    // シェイクの速さ（大きくすると速く振動する）
    public float shakeSpeed = 50f;
    // シェシェイクの継続時間
    public float shakeDuration = 0.5f;

    private bool isShaking = false;//シェイク中かどうかのフラグ
    private bool isOneMinutePassed = false;//1分を切ったかどうかのフラグ
    private Vector3 originalPosition; // テキストの初期位置

    //タイマーの進行状態を管理するフラグ
    private bool _isTimerRunning = true;

    // 初期値（リセット用）を保持する変数
    private float _initialTime;

    private MessageUIController _messageController;// メッセージUIコントローラーへの参照


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //タイマーの変数
    public float GameTimer;
    void Start()
    {
        // インスペクターで設定されているか確認
        if (tmpText != null)
        {
            // 色を白色に
            tmpText.color = Color.white;
        }
        // 初期位置を保存
        if (timeText != null)
        {
            originalPosition = timeText.transform.localPosition;
        }

        // 初期値を保存 (リセット用)
        _initialTime = totalTime;

        _messageController = FindFirstObjectByType<MessageUIController>(FindObjectsInactive.Include);// MessageUIController の参照を取得

    }
    // Update is called once per frame
    void Update()
    {
        if (_isTimerRunning && totalTime > 0f)
        {
          

            // 時間を更新して表示するメソッドを呼び出します
            UpdateTimeDisplay(totalTime);
            CountDownTimerColor(totalTime);//タイマーカラー
                                           // カウントダウン
            totalTime -= Time.deltaTime;

            if (totalTime <= 60f)
            {
                // 60秒を切ったときから、5秒ごとのシェイクをチェックする
                CheckAndStartShake();
            }

            if (totalTime <= 0f)
            {
                // 時間がゼロになったら、一度だけ終了処理を呼び出す
                HandleTimerEnd();
                return; // ここでUpdateを終了し、下の処理は実行しない
            }
        }
    }

    private void HandleTimerEnd()//タイマー0の時の処理メソッド
    {
        // 1. 時間を強制的に0に固定 (念のため)
        totalTime = 0f;

        // 2. タイマー表示を "00:00" に更新
        UpdateTimeDisplay(totalTime);

        // 3. 実行中のすべてのコルーチン（シェイクを含む）を停止
        // このStopAllCoroutines()が、シェイク処理を確実に停止させます
        StopAllCoroutines();

        // 4. テキストの位置を初期位置に戻す
        timeText.transform.localPosition = originalPosition;


        if (_messageController != null)//メッセージUIコントローラーが存在する場合
        {
            _messageController.ShowMessage(MessageUIController.MessageType.TimeUp);//時間切れメッセージを表示
        }


        // 5. ゲームの次のアクション（例：ゲームオーバー画面の表示）
        Debug.Log("タイムアップ！ゲームを停止します。");
        // Time.timeScale = 0f; // ゲーム全体をポーズさせたい場合
    }

    private void CountDownTimerColor(float totalTime)//時間経過でタイマーの色を変更するメソッド
    {
        //残り時間が5分を切るとテキストの色を黄色にする
        if (totalTime <= 300f)
        {
            tmpText.color =Color.orange;
        }
        //残り時間が1分を切るとテキストの色を赤色にする
        if (totalTime <= 60f) 
        {
            tmpText.color = Color.red;
           
        }
    }

    void UpdateTimeDisplay(float timeToDisplay)
    {
        // 1. 分と秒の計算
        // 分（整数部）
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        // 秒（0〜59）
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // 2. 時間フォーマットの適用（「D2」で常に2桁表示を保証）
        // 例: minutes.ToString("D2") が "09" や "10" を返す
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        // TextMesh Proコンポーネントに設定
        if (timeText != null)
        {
            timeText.text = formattedTime;
        }
    }//残り時間に合わせてタイマーテキストを変更するメソッド

    void CheckAndStartShake()//シェイクチェックメソッド
    {
        // 1分を切った瞬間に一度だけ実行される処理（なくても動くが、5秒ごとの処理をシンプルにするため）
        if (!isOneMinutePassed)
        {
            isOneMinutePassed = true;
        }

        // isShaking: 既にシェイク中なら何もしない
        if (!isShaking && Mathf.FloorToInt(totalTime) % 1 == 0)//1秒毎にシェイクさせる
        {
            // totalTimeが5.000...秒のときにシェイクを開始し、
            // totalTimeが4.999...秒になるまで待つことで、
            // 毎フレームシェイクが開始されるのを防ぐ
            if (totalTime % 1.0f < Time.deltaTime * 2)
            {
                // シェイクコルーチンを開始
                StartCoroutine(ShakeText(shakeDuration));
            }
        }
    }//タイマーテキストを動かすメソッド
    IEnumerator ShakeText(float duration)
    {
        isShaking = true;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 経過時間に応じて揺れの強さを決定（最後は揺れを弱める）
            float currentMagnitude = shakeMagnitude * (duration - elapsed) / duration;

            // ランダムな位置を生成（XとY方向）
            // Mathf.PerlinNoiseなどを使ってより滑らかな動きにすることも可能
            float x = Random.Range(-1f, 1f) * currentMagnitude;
            float y = Random.Range(-1f, 1f) * currentMagnitude;

            // テキストの位置を更新
            timeText.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            // 1フレーム待機
            elapsed += Time.deltaTime;
            yield return null;
        }

        // シェイクが終了したら、元の位置に戻す
        timeText.transform.localPosition = originalPosition;
        isShaking = false;
    }//シェイクコルーチン

    public void StopAndResetTimer(bool shouldReset)
    {
        _isTimerRunning = false; // 1. タイマーの進行を停止
        StopAllCoroutines();     // 2. 実行中のシェイクコルーチンを全て停止

        // 3. テキストの位置を初期位置に戻す
        if (timeText != null)
        {
            timeText.transform.localPosition = originalPosition;
        }

        if (shouldReset)
        {
            // 4. タイマーを初期値に戻す
            totalTime = _initialTime;
            // 5. 表示をリセット後の時間に更新
            UpdateTimeDisplay(totalTime);
            // 6. 色を初期状態（白）に戻す
            if (tmpText != null) { tmpText.color = Color.white; }
        }
    }


}
