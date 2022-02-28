using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;

    InventorySlot[] slots;
    Inventory inventory;

    public GameObject inventoryObject;

    public delegate void OnSelectionChange();
    public OnSelectionChange onSelectionChangeCallback;

    // item description
    public Image icon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    private void Start()
    {
        icon.enabled = false;
        title.enabled = false;
        description.enabled = false;

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

    private void UpdateUI()
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

    public void UpdateDisplayItem(Item item)
    {
        if (item != null)
        {
            icon.enabled = true;
            title.enabled = true;
            description.enabled = true;

            icon.sprite = item.icon;
            title.text = item.name;
            description.text = item.description;
        }
        else
        {
            icon.enabled = false;
            title.enabled = false;
            description.enabled = false;
        }
    }
}
