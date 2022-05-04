using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static List<EnemySpawner> instances;

    public Dictionary<EnemyStatsSO, float> enemyTypes = new Dictionary<EnemyStatsSO, float>();

    [SerializeField]
    private GameObject enemyGO;
    private Pool enemyPool;
    public int maxEnemies;

    public int spawnRate;

    private bool spawning;

    private void Awake()
    {
        if (instances == null)
        {
            instances = new List<EnemySpawner>();
        }

        instances.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyPool = new Pool(transform, enemyGO, maxEnemies);
        spawning = true;
        StartCoroutine(SpawnEnemies());
    }

    public void Freeze()
    {
        spawning = false;

        StopAllCoroutines();

        StartCoroutine(FreezeCoroutine());
    }

    private IEnumerator FreezeCoroutine()
    {
        foreach (var enemy in enemyPool.activeObjects)
        {
            enemy.GetComponent<Enemy>().Freeze();
        }

        yield return new WaitForSeconds(3);

        spawning = true;

        foreach (var enemy in enemyPool.activeObjects)
        {
            enemy.GetComponent<Enemy>().Unfreeze();
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (spawning)
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
                // each fifteenth wave the enemies get stronger
                enemy.ApplyStats(GameController.instance.currentWave / 15 + 1);
                enemy.ResetFreeze();
            }
        }
    }
}
