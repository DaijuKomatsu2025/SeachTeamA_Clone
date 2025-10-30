using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    public void OnButtonclick()
    {
        SceneManager.LoadScene("GameScene");
    }
}
