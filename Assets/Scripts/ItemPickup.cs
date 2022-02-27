using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    // References
    public Item item;

    public override void Interact()
    {
        base.Interact();

        // pick the item
        PickUp();
    }

    private void PickUp()
    {
        // add item in the inventory
        Debug.Log("Adding " + item.name + " to the inventory.");
        if (Inventory.instance.Add(item)) // if item is successfully added, we destroy game object
        {
            Destroy(gameObject);
        }
    }
}
