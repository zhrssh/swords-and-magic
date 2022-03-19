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
        public List<GameObject> enemies;
        public int count;
        public float rate;
    }

    [System.Serializable]
    public class Boss
    {
        public string name;
        public GameObject boss;
        public int count;
        public float rate;
    }

    private SpawnState state = SpawnState.Counting;

    public Wave[] waves;
    public Boss[] bosses;
    private int nextWave = 0;
    private int waveNumber = 1;
    public int GetWaveNumber() { return waveNumber; }

    public float timeBetweenWaves = 10f;
    private float waveCountdown;
    public float GetWaveCountdown() { return waveCountdown; }

    private float spawnRadius = 5f;

    // Game State
    [SerializeField] private Player player;
    private bool gameHasEnded = false;

    // Wave multiplier
    private float difficultyMultiplier = 1f;
    [SerializeField] private float difficultyRate = 10f; // the lower the number, the higher the exponential rate

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        gameHasEnded = false;
    }

    private void Update()
    {
        // Check if player is dead
        if (player.IsDead())
            GameEnded();

        if (!gameHasEnded) // if player is not dead we continue
        {
            difficultyMultiplier += Time.deltaTime; // measures how long the player has survived
            if (waveCountdown <= 0)
            {
                if (state != SpawnState.Spawning)
                {
                    // Start spawning waves
                    StartCoroutine(SpawnWave(waves[nextWave]));
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
        nextWave = (nextWave + 1) % waves.GetLength(0);

        // Spawn number of enemies based on count
        for (int i = 0; i < _wave.count; i++)
        {
            // Spawn enemies inside the list of enemies that can spawn in each wave
            SpawnEnemy(_wave.enemies);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        // Spawn Boss every 7th wave
        if (waveNumber % 7 == 0)
        {
            StartCoroutine(SpawnBoss());
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
        _wave.count += Mathf.RoundToInt(difficultyMultiplier / difficultyRate);
    }

    private void AddBossMultiplier(Boss _boss)
    {
        _boss.count += Mathf.RoundToInt(difficultyMultiplier / (difficultyRate * 2));
    }

    private void SpawnEnemy(List<GameObject> enemies)
    {
        // Spawn enemiesaround the player
        Instantiate(enemies[Random.Range(0, enemies.Count)], player.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity);
    }

    private IEnumerator SpawnBoss()
    {
        // choose which boss to spawn
        Boss _boss = bosses[Random.Range(0, bosses.Length)];
        
        if (_boss != null)
        {
            for (int i = 0; i < _boss.count; i++)
            {
                // Spawn the boss
                Instantiate(_boss.boss, player.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity);
                yield return new WaitForSeconds(1f / _boss.rate);
            }

            // Add multiplier
            AddBossMultiplier(_boss);
        }
        else
        {
            Debug.LogWarning("No Boss Set!");
            yield break;
        }
    }

    private void GameEnded()
    {
        // Display Death Screen
        LevelManager.instance.DisplayDeathScreen();
        gameHasEnded = true;
    }
}
