using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    #region Singleton
    private static ObjectPooling _instance;
    public static ObjectPooling instance {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ObjectPooling>();
            }

            return _instance;
        }
    }
    #endregion

    // Object Pooling
    private Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    public void CreatePool(GameObject prefab, int size = 255)
    {
        int key = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(key))
        {
            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;
            
            poolDictionary.Add(key, new Queue<ObjectInstance>());
            for (int i = 0; i < size; i++)
            {
                ObjectInstance obj = new ObjectInstance(Instantiate(prefab) as GameObject); 
                poolDictionary[key].Enqueue(obj);
                obj.SetParent(poolHolder.transform);
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int key = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(key))
        {
            ObjectInstance objToReuse = poolDictionary[key].Dequeue();
            poolDictionary[key].Enqueue(objToReuse);
            objToReuse.Reuse(position, rotation);
        }
    }

    public class ObjectInstance
    {
        GameObject gameObject;
        Transform transform;

        bool hasPoolObjComponent;
        PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
        }

        public void Reuse(Vector3 position, Quaternion rotation)
        {
            if (hasPoolObjComponent)
                poolObjectScript.OnObjectReuse();


            gameObject.SetActive(true);
            transform.position = position;
            transform.rotation = rotation;
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
}
