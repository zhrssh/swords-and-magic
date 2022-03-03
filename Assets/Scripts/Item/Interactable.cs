using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 0.15f;

    public virtual void Interact()
    {
        // this method is meant to be overridden
    }
}
