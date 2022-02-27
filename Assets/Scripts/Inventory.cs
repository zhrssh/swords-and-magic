using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public int inventorySpace = 8;
    public List<Item> items = new List<Item>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found!");
            return;
        }

        instance = this;
    }

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count < inventorySpace)
            {
                items.Add(item);

                // Used to update ui
                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();

                return true; // meaning the item is picked up
            }
            else
            {
                Debug.Log("No more inventory space!");
                return false; // the item is not picked up
            }
        }

        return false;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
