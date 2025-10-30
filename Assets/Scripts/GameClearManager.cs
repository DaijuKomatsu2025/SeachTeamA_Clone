using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameClearManager : MonoBehaviour
{
    [Header("サウンドコンポーネント")]
    // BGM追加用のサウンドコンポーネント
    [SerializeField] private AudioSource bgmAudioSource;

    [Header("アニメーション対象")]
    // 🏆 GameClearText (TMP) にアタッチされているAnimatorを設定
    [SerializeField] private Animator animator;

    //アニメーションの時間
    [SerializeField]
    private float animationDuration = 5.0f;

    // 🏆 このStart()メソッドが「ゲームクリアシーンがロードされた瞬間」に自動で実行されます 🏆
    void Start()
    {
        Time.timeScale = 1.0f;
        // 1. BGMを再生
        // 1. BGMを再生
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Play();
        }
        // 非推奨の Physics.autoSimulation = false; を新しいAPIに置き換え
        Physics.simulationMode = SimulationMode.Script;

        // 🏆 初期化の大部分を次のフレームに遅延させる 🏆
        StartCoroutine(InitializeNextFrame());
    }

    private IEnumerator InitializeNextFrame()
    {
        // 1フレーム待機して、Start()の初期化が完了するのを待つ
        yield return null;

        // TextMeshProUGUI コンポーネントを取得
        TMPro.TextMeshProUGUI tmpText;
        if (animator.TryGetComponent<TMPro.TextMeshProUGUI>(out tmpText))
        {
            // TMPの描画強制
            tmpText.ForceMeshUpdate();
        }

        // アニメーションを起動
        if (animator != null)
        {
            animator.SetTrigger("GameClear");
        }

        // アニメーション終了待ちコルーチンを開始
        StartCoroutine(WaitForAnimationEnd());
    }

    private IEnumerator WaitForAnimationEnd()
    {
        // アニメーションの再生時間分だけ待機
        yield return new WaitForSeconds(animationDuration);

        // 🏆 ここからが終了処理 🏆

        // Unityエディターでの再生停止
#if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying を使うには、
        // スクリプトの冒頭に using UnityEditor; が必要ですが、
        // ビルド時にエラーになるため、#if UNITY_EDITOR で囲って安全に処理します。

        // EditorApplication.isPlaying = false;
        // 💡 ただし、EditorApplication は UnityEditor 名前空間が必要なため、
        // 以下の Debug.Break() を使うのが最もシンプルで安全です。

        Debug.Break();
#endif

        // 実際のゲームビルド時の終了処理（必要なら）
        // Application.Quit(); 
    }
}