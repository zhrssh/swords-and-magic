using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 0.15f;

    bool isIconEnabled = false;

    private void Update()
    {
        if (isIconEnabled)
        {
            // display icon
            Debug.Log("Displaying Interactable Icon On " + name);
        }
        else
        {
            // disable icon
        }
    }

    public virtual void Interact()
    {
        // this method is meant to be overridden
    }

    public void EnableInteractableIcon()
    {
        isIconEnabled = true;
    }

    public void DisableInteractableIcon()
    {
        isIconEnabled = false;
    }
}
