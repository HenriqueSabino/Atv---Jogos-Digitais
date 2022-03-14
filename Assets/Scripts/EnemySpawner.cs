using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private List<EnemyStatsSO> enemyTypes;

    [SerializeField]
    private GameObject enemyGO;
    private Pool enemyPool;

    [SerializeField]
    public int maxEnemies;


    [SerializeField]
    private int spawnRate;


    // Start is called before the first frame update
    void Start()
    {
        enemyPool = new Pool(transform, enemyGO, maxEnemies);
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds((spawnRate + Random.Range(-100, 100)) / 1000f);

            GameObject enemyGO = enemyPool.Spawn(transform.position, Quaternion.identity);
            Enemy enemy = enemyGO?.GetComponent<Enemy>();

            if (enemy != null)
                enemy.stats = enemyTypes[Random.Range(0, enemyTypes.Count)];
        }
    }
}
