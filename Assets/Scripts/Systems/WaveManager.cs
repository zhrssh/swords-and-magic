using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemies are instantiated in a pool at the start of the game for performance.
// Enemies are spawned after a certain countdown.
// Enemies are recycled when dead.
// Enemies are spawned after the player has cleared all the enemies
// Enemies multiply after each wave

public class WaveManager : MonoBehaviour
{
    #region Singleton

    private static WaveManager _instance;
    public static WaveManager instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<WaveManager>();
            }

            return _instance;
        }
    }

    #endregion

    [System.Serializable]
    public struct Wave
    {
        public GameObject prefab;
        public int objToSpawnInWave;
    }

    private enum WaveState
    {
        Counting,
        Spawning,
        Waiting
    }

    // Wave Properties
    [SerializeField] private Wave[] waves;
    
    [Header("Properties")]
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float spawnRadius;

    // Player reference
    Player player;

    // System
    private WaveState state;
    private int nextWave = 0;
    public float waveCountdown;
    public int waveSurvived;
    private float timeSurvived;
    private float difficultyMultiplier = 0.1f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        for (int i = 0; i < waves.Length; i++)
        {
            if (waves[i].prefab != null)
                ObjectPooling.instance.CreatePool(waves[i].prefab);
        }

        waveCountdown = timeBetweenWaves;
        state = WaveState.Counting;
    }

    private void Update()
    {
        if (!IsPlayerDead())
        {
            timeSurvived += Time.deltaTime;
            if (state == WaveState.Waiting)
            {
                if (AreEnemiesDead())
                {
                    // if no enemies left
                    waveSurvived++;
                    waveCountdown = timeBetweenWaves;
                    state = WaveState.Counting;
                }
            }

            if (state == WaveState.Counting)
            {
                // countdown til the next wave
                waveCountdown -= Time.deltaTime;
                
                if (waveCountdown <= 0f)
                {
                    StartCoroutine(SpawnWave());
                }
            }
        }
    }

    private bool IsPlayerDead()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            return true;
        }

        return false;
    }

    private bool AreEnemiesDead()
    {
        if (GameObject.FindGameObjectWithTag("Killable") == null)
        {
            return true;
        }

        return false;
    }

    private IEnumerator SpawnWave()
    {
        state = WaveState.Spawning;
        for (int i = 0; i < waves[nextWave].objToSpawnInWave; i++)
        {
            ObjectPooling.instance.ReuseObject(
                waves[nextWave].prefab, 
                player.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)), 
                Quaternion.identity
                );

            yield return new WaitForSeconds(.1f);
        }

        // After spawning the waves, set the previous wave's difficulty
        AddDifficulty();
        state = WaveState.Waiting;
        nextWave = (nextWave + 1) % waves.Length;
        yield break;
    }

    private void AddDifficulty()
    {
        waves[nextWave].objToSpawnInWave += Mathf.FloorToInt(timeSurvived * difficultyMultiplier);
    }
}
