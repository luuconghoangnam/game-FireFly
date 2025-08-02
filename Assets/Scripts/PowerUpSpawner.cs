using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    public static PowerUpSpawner Instance;
    
    [SerializeField] private float minSpawnTime = 15f;
    [SerializeField] private float maxSpawnTime = 30f;
    [SerializeField] private float spawnWidth = 8f;
    
    private bool isSpawning = false;
    private string[] powerUpTypes = { "PowerUpRapidFire", "PowerUpDoubleDamage", "PowerUpShield" };
    
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
        StartCoroutine(SpawnPowerUps());
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
    
    IEnumerator SpawnPowerUps()
    {
        while (isSpawning)
        {
            // Đợi thời gian ngẫu nhiên
            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnDelay);
            
            // Chọn power-up ngẫu nhiên
            string powerUpTag = powerUpTypes[Random.Range(0, powerUpTypes.Length)];
            
            // Chọn vị trí ngẫu nhiên
            float randomX = Random.Range(-spawnWidth / 2, spawnWidth / 2);
            Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0f);
            
            // Spawn power-up
            GameObject powerUp = ObjectPooler.Instance.GetPooledObject(powerUpTag);
            if (powerUp != null)
            {
                powerUp.transform.position = spawnPosition;
                powerUp.SetActive(true);
            }
        }
    }
}
