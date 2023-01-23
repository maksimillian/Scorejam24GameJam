using System;
using UnityEngine;

public class HorseShellBehavior : MonoBehaviour, IShell
{
    private EnemyProfiler enemyProfiler;
    private GameObject enemyPrefab;
 
    public void Init(Sprite sprite)
    {
        enemyProfiler = Resources.Load<EnemyProfiler>("Enemies/EnemyTypes/Bomber");
        enemyPrefab = Resources.Load<GameObject>("Enemies/Enemy");
    }

    public void Fire(Vector3 targetDirection)
    {
        Debug.Log("SpawnEnemy from horse");

        Vector3 spawnPos = transform.position;

        var enemy = Instantiate(enemyPrefab, spawnPos, transform.rotation);
        enemy.GetComponent<EnemyBehavior>().Init(enemyProfiler);
        enemy.transform.localScale = new Vector3(enemyProfiler.enemySize, enemyProfiler.enemySize, enemyProfiler.enemySize);
        SelfDestroy();
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
