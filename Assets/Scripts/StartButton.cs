using UnityEngine;
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
