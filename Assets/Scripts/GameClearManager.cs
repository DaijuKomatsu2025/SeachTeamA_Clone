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
        // 1. BGMを再生
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Play();
        }

        // TextMeshProUGUI コンポーネントを取得
        TMPro.TextMeshProUGUI tmpText = animator.GetComponent<TMPro.TextMeshProUGUI>();

        // 2. アニメーションを起動
        if (animator != null) // AnimatorコンポーネントがInspectorで設定されているか確認
        {
            // Triggerを引くことで、待機状態(Idle)からGameClearアニメーションへ即座に遷移
            animator.SetTrigger("GameClear");
        }
        //// アニメーションの終了を待ってから終了処理を呼び出す
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