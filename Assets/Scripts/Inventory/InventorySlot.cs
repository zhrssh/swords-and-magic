using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button useButton;
    public Button dropButton;
    public Button closeButton;
    
    Item item;

    InventoryUI inventoryUI;

    private void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
            inventoryUI.onSelectionChangeCallback += DisableOptions;

        DisableOptions();
    }

    public void AddItem(Item newItem)
    {
        // Uses the data from the item and displays it in the game
        item = newItem;
        icon.sprite = item.sprite;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;

        DisableOptions();
    }

    public void OnSlotClicked()
    {
        if (item != null) // if there is an item
        {
            // we show the options to use and drop the item
            EnableOptions();
        }

        // this function is to be able to open the options on one slot
        if (inventoryUI.onSelectionChangeCallback != null)
        {
            // we temporarily remove this function then call the disable options
            inventoryUI.onSelectionChangeCallback -= DisableOptions; 
            inventoryUI.onSelectionChangeCallback.Invoke();
            inventoryUI.onSelectionChangeCallback += DisableOptions;
        }

        // we show the item description, icon and name in the display
        inventoryUI.UpdateDisplayItem(item);
    }

    public void OnCloseButtonClicked()
    {
        if (item != null)
        {
            DisableOptions();
        }
    }

    public void OnItemRemoved()
    {
        if (item != null)
        {
            // we remove the item and clear the slot
            Debug.Log("Removed: " + item.name);
            Inventory.instance.Remove(item);

            // after removing the item we close the options
            DisableOptions();

            // We update the display
            inventoryUI.UpdateDisplayItem(item);
        }
    }

    public void OnItemUsed()
    {
        if (item != null)
        {
            item.Use(); // we use the item
        }

        // We update the display
        inventoryUI.UpdateDisplayItem(item);
    }

    void EnableOptions()
    {
        useButton.gameObject.SetActive(true);
        dropButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);

        useButton.interactable = true;
        dropButton.interactable = true;
        closeButton.interactable = true;
    }

    void DisableOptions()
    {
        useButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);

        useButton.interactable = false;
        dropButton.interactable = false;
        closeButton.interactable = false;
    }
}
