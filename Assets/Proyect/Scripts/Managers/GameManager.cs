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
        // Handle game state transitions or updates here if needed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentGameState == GameState.Playing)
            {
                NewGameState(GameState.Paused);
            }
            else if (CurrentGameState == GameState.Paused)
            {
                NewGameState(GameState.Playing);
            }
            
        }
    }

    public void NewGameState(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.MainMenu:
                // Handle main menu state
                break;
            case GameState.Playing:
                // Handle playing state
                break;
            case GameState.Paused:
                // Handle paused state
                break;
            case GameState.GameOver:
                // Handle game over state
                break;
        }

        CurrentGameState = newGameState;
        Debug.Log("Game state changed to: " + CurrentGameState);
    }
}

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}
