using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;

    InventorySlot[] slots;
    Inventory inventory;

    public GameObject inventoryObject;

    private void Start()
    {
        // temporary
        inventoryObject.SetActive(false);

        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>(); // gets reference to all the inventory slots
    }

    private void Update()
    {
        // enables and disables inventory ui. this is temporary
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryObject.SetActive(!inventoryObject.activeSelf);
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++) // loops through all the slots
        {
            if (i < inventory.items.Count) // check if there are more items to add
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
