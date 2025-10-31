using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    [SerializeField] private Button startButton;

    public void OnButtonclick()
    {
        startButton.interactable = false;
        SceneReloader.Instance.loadGameScene("GameScene");
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveAllListeners();
    }
}
