using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Dictionary<EnemyStatsSO, float> enemyTypes = new Dictionary<EnemyStatsSO, float>();

    [SerializeField]
    private GameObject enemyGO;
    private Pool enemyPool;
    public int maxEnemies;

    public int spawnRate;

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
            {
                // Randomly selects an enemy
                float rnd = Random.Range(0f, 1f);
                int index = -1;
                do
                {
                    rnd -= enemyTypes.Values.ElementAt(++index);
                } while (rnd > 0);

                enemy.stats = enemyTypes.Keys.ElementAt(index);
            }
        }
    }
}
