using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    #region Singleton

    public static ObjectPooling instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    #endregion

    // Object Pooling
    private Dictionary<string, Queue<GameObject>> pool;
    [SerializeField] int size;

    // Wave Manager
    private WaveManager waveManager;

    private void Start()
    {
        waveManager = WaveManager.instance;
        pool = new Dictionary<string, Queue<GameObject>>();
        
        // Handles the enqueuing of the enemies in the queue and adding in the pool dictionary
        for (int i = 0; i < waveManager.waves.Length; i++)
        {
            WaveManager.Wave currentWave = waveManager.waves[i];

            // Add each wave name to the dictionary and initialize with a new queue
            Queue<GameObject> q = new Queue<GameObject>();
            for (int j = 0; j < size; j++)
            {
                // Instantiates the objects inside the wave and add it to the queue
                if (currentWave.enemy != null)
                {
                    GameObject obj = Instantiate(currentWave.enemy);
                    obj.SetActive(false);
                    obj.transform.parent = gameObject.transform;
                    q.Enqueue(obj);
                }
            }

            pool.Add(currentWave.name, q);
        }
    }

    public Dictionary<string, Queue<GameObject>> GetDictionary()
    {
        return pool;
    }
}
