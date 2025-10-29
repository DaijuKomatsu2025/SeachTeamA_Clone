using UnityEngine;

public class GameClearManager : MonoBehaviour
{
    [Header("サウンドコンポーネント")]
    // BGM追加用のサウンドコンポーネント
    [SerializeField] private AudioSource bgmAudioSource;

    [Header("アニメーション対象")]
    // 🏆 GameClearText (TMP) にアタッチされているAnimatorを設定
    [SerializeField] private Animator animator;

    // 🏆 このStart()メソッドが「ゲームクリアシーンがロードされた瞬間」に自動で実行されます 🏆
    void Start()
    {
        // 1. BGMを再生
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Play();
        }

        // 2. アニメーションを起動
        if (animator != null) // AnimatorコンポーネントがInspectorで設定されているか確認
        {
            // Triggerを引くことで、待機状態(Idle)からGameClearアニメーションへ即座に遷移
            animator.SetTrigger("GameClear");
        }
    }
}