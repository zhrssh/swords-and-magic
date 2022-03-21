using UnityEngine;

public interface IObjectPooled
{
    void OnObjectSpawned(Vector3 position, Quaternion rotation);
}
