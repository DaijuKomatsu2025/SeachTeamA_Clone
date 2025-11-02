using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private ImageAlphaLerper _fadeLerper;
    [SerializeField] private AudioClip _desideSound;

    public void OnButtonclick()
    {
        if (_desideSound != null)
        {
            SoundManager.Instance.PlaySound(_desideSound);
        }
        StartCoroutine(EnableButtonAfterDelay(1f));
    }

    IEnumerator EnableButtonAfterDelay(float delay)
    {
        // 完全に表示（不透明）にフェードイン
        _fadeLerper.FadeTo(delay);
        yield return new WaitForSeconds(delay + 1f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif PLATFORM_STANDALONE
        Application.Quit();
#endif
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveAllListeners();
    }
}
