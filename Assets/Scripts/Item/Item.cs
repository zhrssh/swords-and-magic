using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public string description = "This is the item description.";
    public Sprite inventoryIcon = null;
    public Sprite sprite = null;
    public bool isDefaultItem = false;

    public virtual void Use(Player owner)
    {
        // method meant to be overwritten
    }

    public void RemoveItemFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
