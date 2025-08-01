using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    
    public GameObject GetPooledObject(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;
            
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        
        if (objectToSpawn.activeInHierarchy)
        {
            // If all objects are in use, create a new one
            objectToSpawn = Instantiate(pools.Find(p => p.tag == tag).prefab);
        }
        
        poolDictionary[tag].Enqueue(objectToSpawn);
        
        return objectToSpawn;
    }
}
