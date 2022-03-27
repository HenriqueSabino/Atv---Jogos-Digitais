using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    public Dictionary<BonusSO, float> bonusTypes = new Dictionary<BonusSO, float>();

    [SerializeField]
    private GameObject bonusGO;
    private Pool bonusPool;
    public int maxBonuses;

    public int spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        bonusPool = new Pool(transform, bonusGO, maxBonuses);
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds((spawnRate + Random.Range(-100, 100)) / 1000f);

            GameObject enemyGO = bonusPool.Spawn(transform.position, Quaternion.identity);
            Bonus bonus = enemyGO?.GetComponent<Bonus>();

            if (bonus != null)
            {
                // Randomly selects an enemy
                float rnd = Random.Range(0f, 1f);
                int index = -1;
                do
                {
                    rnd -= bonusTypes.Values.ElementAt(++index);
                } while (rnd > 0);

                bonus.bonusSO = bonusTypes.Keys.ElementAt(index);
                bonus.ApplyStats();
            }
        }
    }
}
