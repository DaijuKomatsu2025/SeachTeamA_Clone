using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    [SerializeField] private Button _exitButton;

    public void OnButtonclick()
    {
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
