using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // The enemy prefab that will be spawned
    [SerializeField] private float spawnInterval = 3f; // The interval at which enemies will be spawned
    [SerializeField] private float spawnRadius = 300f; // The maximum distance from the spawner that the enemy can spawn
    
    [SerializeField] private List<EnemyProfilersInfo> enemyProfilers;

    private float spawnTimer = 0f; // A timer to track when to spawn the next enemy

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy();
        }
    }

    void Update()
    {
        spawnTimer += Time.deltaTime; // Increase the spawn timer by the time since the last frame

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnInterval *= 0.999f;            
            spawnTimer = 0f; // Reset the spawn timer
        }
    }

    void SpawnEnemy()
    {
        Debug.Log("SpawnEnemy");
        var enemyProfiler = GetRandomEnemyProfiler();
        
        Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPos = new Vector3(spawnPos.x, spawnPos.y, 0);
        //Vector3 spawnPos = transform.position;
        
        
        var enemy = Instantiate(enemyPrefab, spawnPos, transform.rotation);
        enemy.transform.localScale = new Vector3(enemyProfiler.enemySize, enemyProfiler.enemySize, enemyProfiler.enemySize);
        enemy.GetComponent<EnemyBehavior>().Init(enemyProfiler);
    }

    private EnemyProfiler GetRandomEnemyProfiler()
    {
        int totalChances = 0;
        int[] chances = new int[enemyProfilers.Count];
        for (int i = 0; i < enemyProfilers.Count; i++)
        {
            totalChances += enemyProfilers[i].spawnChance;
            chances[i] = totalChances;
        }
        int randomValue = Random.Range(0, totalChances);
        for (int i = 0; i < chances.Length; i++)
        {
            if (randomValue < chances[i])
            {
                return enemyProfilers[i].profiler;
            }
        }
        return null;
    }
}

[Serializable]
public class EnemyProfilersInfo
{
    public int spawnChance;
    public EnemyProfiler profiler;
}
