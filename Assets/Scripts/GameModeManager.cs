using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager instance;

    void Awake()
    {
        if (instance is null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public enum GameMode
    {
        Title,
        Explore,
        Annihilate,
        TimeOver,
        GameOver,
    }

    public static GameMode currentMode { get; set; } = GameMode.Explore;

    public static void SetGameMode(GameMode mode) => currentMode = mode;
}