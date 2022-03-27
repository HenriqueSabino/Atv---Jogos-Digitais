using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Enemies")]

    [SerializeField]
    private List<EnemySpawner> enemySpawners;
    public List<Wave<EnemyStatsSO>> enemyWaves;

    [SerializeField]
    private int initialEnemySpawnTimer;

    [SerializeField]
    private int minEnemySpawnTimer;
    [SerializeField]
    private int enemySpawntimerDecrement;

    [Header("Bonus")]

    [SerializeField]
    private List<BonusSpawner> bonusSpawners;
    public List<Wave<BonusSO>> bonusWaves;

    [SerializeField]
    private int initialBonusSpawnTimer;

    [Header("General")]

    [SerializeField]
    private int timePerWave;
    public bool Paused;

    public int currentWave { get; private set; }

    public UnityEvent waveChanged;
    public UnityEvent<bool> gamePaused;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;

        foreach (var spawner in enemySpawners)
        {
            spawner.spawnRate = initialEnemySpawnTimer;
        }

        foreach (var spawner in bonusSpawners)
        {
            spawner.spawnRate = initialBonusSpawnTimer;
        }

        StartCoroutine(PlayWave());
    }

    public void TogglePause()
    {
        if (Paused)
        {
            Time.timeScale = 1;
            Paused = false;
        }
        else
        {
            Time.timeScale = 0;
            Paused = true;
        }

        gamePaused.Invoke(Paused);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private IEnumerator PlayWave()
    {
        while (true)
        {
            // Takes the last wave specified in waves that matches the current wave
            Wave<EnemyStatsSO> enemyWave = enemyWaves.Last(w => w.waveNumber <= currentWave);

            Wave<BonusSO> bonusWave = bonusWaves.Last(w => w.waveNumber <= currentWave);

            foreach (var spawner in enemySpawners)
            {
                spawner.enemyTypes.Clear();
                foreach (var enemy in enemyWave.objects)
                {
                    spawner.enemyTypes.Add(enemy.objectType, enemy.probability);
                }
            }

            foreach (var spawner in bonusSpawners)
            {
                spawner.bonusTypes.Clear();
                foreach (var bonus in bonusWave.objects)
                {
                    spawner.bonusTypes.Add(bonus.objectType, bonus.probability);
                }
            }

            yield return new WaitForSeconds(timePerWave / 1000f);

            foreach (var spawner in enemySpawners)
            {
                spawner.spawnRate -= enemySpawntimerDecrement;
            }

            currentWave++;
            waveChanged.Invoke();
        }
    }
}

[System.Serializable]
public struct WaveObject<T>
{
    public T objectType;
    public float probability;
}


[System.Serializable]
public struct Wave<T>
{
    public int waveNumber;
    public List<WaveObject<T>> objects;
}