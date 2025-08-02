using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject playerShip;
    [SerializeField] private Transform playerStartPosition;
    
    private bool isGameActive = false;
    private float gameTime = 0f;
    private int currentScore = 0;
    private int highScore = 0;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        // Load high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
    
    void Update()
    {
        if (isGameActive)
        {
            gameTime += Time.deltaTime;
            UIManager.Instance.UpdateGameTime(gameTime);
        }
    }
    
    public void StartGame()
    {
        isGameActive = true;
        currentScore = 0;
        gameTime = 0f;
        
        UIManager.Instance.UpdateScore(currentScore);
        playerShip.SetActive(true);
        playerShip.transform.position = playerStartPosition.position;
        
        // Start spawning enemies
        EnemySpawner.Instance.StartSpawning();
        
        // Start spawning obstacles
        ObstacleSpawner.Instance.StartSpawning();
        
        // Start spawning power-ups
        PowerUpSpawner.Instance.StartSpawning();
        
        // Reset difficulty
        DifficultyManager.Instance.ResetDifficulty();
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        UIManager.Instance.UpdateScore(currentScore);
    }
    
    public void GameOver()
    {
        isGameActive = false;
        EnemySpawner.Instance.StopSpawning();
        ObstacleSpawner.Instance.StopSpawning();
        
        // Stop spawning power-ups
        PowerUpSpawner.Instance.StopSpawning();
        
        // Update high score if needed
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        
        UIManager.Instance.ShowGameOverScreen(currentScore, highScore);
    }
    
    public bool IsGameActive()
    {
        return isGameActive;
    }
    
    public int GetCurrentScore()
    {
        return currentScore;
    }
}
