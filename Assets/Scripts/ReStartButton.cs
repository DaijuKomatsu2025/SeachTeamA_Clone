using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReStartButton : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    public void OnButtonclick()
    {
        GetComponent<SceneReloader>().ReloadSceneClean("GameScene");
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveAllListeners();
    }
}
