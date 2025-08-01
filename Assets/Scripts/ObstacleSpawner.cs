using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner Instance;
    
    [SerializeField] private float spawnRate = 3f;
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
        StartCoroutine(SpawnObstacles());
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
    
    IEnumerator SpawnObstacles()
    {
        while (isSpawning)
        {
            SpawnObstacle();
            yield return new WaitForSeconds(spawnRate);
        }
    }
    
    void SpawnObstacle()
    {
        float randomX = Random.Range(-spawnWidth / 2, spawnWidth / 2);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0f);
        
        GameObject obstacle = ObjectPooler.Instance.GetPooledObject("Obstacle");
        if (obstacle != null)
        {
            obstacle.transform.position = spawnPosition;
            obstacle.transform.rotation = Quaternion.identity;
            obstacle.SetActive(true);
        }
    }
}