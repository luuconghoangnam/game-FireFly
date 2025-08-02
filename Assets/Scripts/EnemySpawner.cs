using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    
    [SerializeField] private float spawnWidth = 8f;
    
    private bool isSpawning = false;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnEnemies());
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
    
    IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            SpawnEnemy();
            
            // Sử dụng DifficultyManager nếu có, ngược lại sử dụng giá trị mặc định 1.5f
            float currentSpawnRate = 1.5f; // Giá trị mặc định
            
            if (DifficultyManager.Instance != null)
            {
                currentSpawnRate = DifficultyManager.Instance.GetCurrentSpawnRate();
            }
                                     
            yield return new WaitForSeconds(currentSpawnRate);
        }
    }
    
    void SpawnEnemy()
    {
        float randomX = Random.Range(-spawnWidth / 2, spawnWidth / 2);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0f);
        
        GameObject enemy = ObjectPooler.Instance.GetPooledObject("Enemy");
        if (enemy != null)
        {
            enemy.transform.position = spawnPosition;
            
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null && DifficultyManager.Instance != null)
            {
                enemyComponent.SetSpeed(DifficultyManager.Instance.GetCurrentEnemySpeed());
            }
            
            enemy.SetActive(true);
        }
    }
}
