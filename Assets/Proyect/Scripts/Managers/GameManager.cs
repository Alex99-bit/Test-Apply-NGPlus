using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This script is responsible for managing the game state and flow
    public static GameManager Instance;
    public GameState CurrentGameState;

    private void Awake()
    {
        // Ensure that there is only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGameState()
    {
        
    }
}

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}
