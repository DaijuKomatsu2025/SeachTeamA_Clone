using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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