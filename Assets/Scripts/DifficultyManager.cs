using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;
    
    [SerializeField] private float initialSpawnRate = 1.5f;
    [SerializeField] private float minimumSpawnRate = 0.5f;
    [SerializeField] private float spawnRateDecreasePerMinute = 0.2f;
    
    [SerializeField] private float initialEnemySpeed = 2f;
    [SerializeField] private float maximumEnemySpeed = 4f;
    [SerializeField] private float enemySpeedIncreasePerMinute = 0.5f;
    
    private float gameTime = 0;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    void Update()
    {
        if (GameManager.Instance.IsGameActive())
        {
            gameTime += Time.deltaTime;
        }
    }
    
    public float GetCurrentSpawnRate()
    {
        float minutesPlayed = gameTime / 60f;
        float currentSpawnRate = initialSpawnRate - (minutesPlayed * spawnRateDecreasePerMinute);
        return Mathf.Max(currentSpawnRate, minimumSpawnRate);
    }
    
    public float GetCurrentEnemySpeed()
    {
        float minutesPlayed = gameTime / 60f;
        float currentSpeed = initialEnemySpeed + (minutesPlayed * enemySpeedIncreasePerMinute);
        return Mathf.Min(currentSpeed, maximumEnemySpeed);
    }
    
    public void ResetDifficulty()
    {
        gameTime = 0;
    }
}