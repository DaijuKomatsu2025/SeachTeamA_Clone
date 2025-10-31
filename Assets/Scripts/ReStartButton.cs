using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReStartButton : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    public void OnButtonclick()
    {
        restartButton.interactable = false;
        SceneReloader.Instance.loadGameScene("GameScene");
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveAllListeners();
    }
}
