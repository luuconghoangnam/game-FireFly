using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    
    [SerializeField] private float spawnRate = 1f;
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
            yield return new WaitForSeconds(spawnRate);
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
            enemy.SetActive(true);
        }
    }
}
