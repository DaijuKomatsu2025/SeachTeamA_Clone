using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    [SerializeField] private Button startButton;

    public void OnButtonclick()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveAllListeners();
    }
}
