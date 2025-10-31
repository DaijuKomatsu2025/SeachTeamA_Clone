using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public static SceneReloader Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void loadGameScene(string targetScene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(targetScene);
    }

    public void loadGameScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void ReloadSceneClean(string targetScene)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadCleanScene(targetScene));
    }

    public void ReloadSceneClean()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadCleanScene("GameScene"));
    }

    IEnumerator LoadCleanScene(string targetScene)
    {
        var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (var go in allObjects)
        {
            if (go.scene.name == "DontDestroyOnLoad")
            {
                Destroy(go);
            }
        }

        Debug.Log("Destroyed DontDestroyOnLoad objects.");

        // 1フレーム待ってからシーンをロード
        yield return null;

        SceneManager.LoadScene(targetScene);
        Debug.Log("Loading scene: " + targetScene);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif PLATFORM_STANDALONE
        Application.Quit();
#endif
    }
}
