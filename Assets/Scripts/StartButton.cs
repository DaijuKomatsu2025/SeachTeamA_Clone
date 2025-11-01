using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private MenuSelector _menuSelector;
    [SerializeField] private ImageAlphaLerper _fadeLerper;

    public void OnButtonclick()
    {
        Debug.Log("StartButton clicked");
        _startButton.interactable = false;
        _menuSelector.enabled = false;
        StartCoroutine(EnableButtonAfterDelay(1f));
    }

    IEnumerator EnableButtonAfterDelay(float delay)
    {
        // 完全に表示（不透明）にフェードイン
        _fadeLerper.FadeTo(2f);
        yield return new WaitForSeconds(delay);
        SceneReloader.Instance.loadGameScene("GameScene");
    }

    private void OnEnable()
    {
        _startButton.Select();
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveAllListeners();
    }
}
