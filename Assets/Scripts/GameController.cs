using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField]
    private List<EnemySpawner> spawners;
    public List<Wave> waves;

    [SerializeField]
    private int initialSpawnTimer;

    [SerializeField]
    private int minSpawnTimer;

    [SerializeField]
    private int timePerWave;

    [SerializeField]
    private int timerDecrement;

    public int currentWave { get; private set; }

    public UnityEvent waveChanged;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;

        foreach (var spawner in spawners)
        {
            spawner.spawnRate = initialSpawnTimer;
        }

        PlayWave();
    }

    private async void PlayWave()
    {
        // Takes the last wave specified in waves that matches the current wave
        Wave wave = waves.Last(w => w.waveNumber <= currentWave);

        foreach (var spawner in spawners)
        {
            spawner.enemyTypes.Clear();
            foreach (var enemy in wave.enemy)
            {
                spawner.enemyTypes.Add(enemy.enemyType, enemy.probability);
            }
        }

        await Task.Delay(timePerWave);

        foreach (var spawner in spawners)
        {
            spawner.spawnRate -= timerDecrement;
        }

        currentWave++;
        waveChanged.Invoke();
        PlayWave();
    }
}

[System.Serializable]
public struct WaveEnemy
{
    public EnemyStatsSO enemyType;
    public float probability;
}


[System.Serializable]
public struct Wave
{
    public int waveNumber;
    public List<WaveEnemy> enemy;
}