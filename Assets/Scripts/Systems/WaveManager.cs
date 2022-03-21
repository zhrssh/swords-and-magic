using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    #region Singleton

    public static WaveManager instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    #endregion

    public enum SpawnState
    {
        Counting,
        Spawning
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject enemy;
        public int count;
        public float rate;
    }

    private SpawnState state = SpawnState.Counting;

    public Wave[] waves;

    private int nextWave = 0;
    private int waveNumber = 1;
    public int GetWaveNumber() { return waveNumber; }

    public float timeBetweenWaves = 20f;
    private float waveCountdown;
    public float GetWaveCountdown() { return waveCountdown; }

    private float spawnRadius = 5f;

    // Game State
    [SerializeField] private Player player;
    private bool gameHasEnded = false;

    // Wave multiplier
    private float difficultyMultiplier = 1f;
    [SerializeField] private float difficultyRate = 10f; // the lower the number, the higher the exponential rate

    // Object Pooling
    ObjectPooling objectPooling;
    Dictionary<string, Queue<GameObject>> pool;

    private void Start()
    {
        objectPooling = ObjectPooling.instance;
        pool = objectPooling.GetDictionary();

        waveCountdown = timeBetweenWaves;
        gameHasEnded = false;
    }

    private void Update()
    {
        // Check if player is dead
        if (!gameHasEnded && player.IsDead())
            GameEnded();

        if (!gameHasEnded) // if player is not dead we continue
        {
            difficultyMultiplier += Time.deltaTime / difficultyRate; // measures how long the player has survived
            if (waveCountdown <= 0)
            {
                if (state != SpawnState.Spawning)
                {
                    // Start spawning waves
                    nextWave = 0;
                    StartCoroutine("SpawnWave", waves[nextWave]);
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }

            waveCountdown = Mathf.Clamp(waveCountdown, 0, timeBetweenWaves);
        }
    }

    private IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Wave: " + _wave.name);
        state = SpawnState.Spawning;
        waveNumber++;

        // Sets the next wave
        nextWave = (nextWave + 1) % waves.Length;

        // Spawn number of enemies based on count
        for (int i = 0; i < _wave.count; i++)
        {
            // Spawn enemies inside the list of enemies that can spawn in each wave
            StartCoroutine(SpawnEnemy(_wave.name));
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        // Keep spawning waves and add difficulty multiplier
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        // Add multiplier in the wave
        AddMultiplier(_wave);
        
        yield break;
    }

    private void AddMultiplier(Wave _wave)
    {
        _wave.count += Mathf.RoundToInt(difficultyMultiplier);
    }

    private IEnumerator SpawnEnemy(string _wave)
    {
        // To avoid null reference
        if (pool == null)
            pool = ObjectPooling.instance.GetDictionary();

        // Gets the object from the dictionary and spawns it around the player
        GameObject obj = pool[_wave].Dequeue();
        Enemy enemy;

        yield return new WaitForSeconds(.1f);

        if (obj.TryGetComponent<Enemy>(out enemy))
        {
            Vector3 randPos = new Vector3(player.transform.position.x + Random.Range(-spawnRadius, spawnRadius), player.transform.position.y + Random.Range(-spawnRadius, spawnRadius));
            enemy.OnObjectSpawned(randPos, Quaternion.identity);
        }

        pool[_wave].Enqueue(obj);
    }

    private void GameEnded()
    {
        // Display Death Screen
        LevelManager.instance.DisplayDeathScreen();
        gameHasEnded = true;
    }
}
