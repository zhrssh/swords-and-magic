using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button useButton;
    public Button dropButton;
    public Button closeButton;
    
    Item item;

    private void Start()
    {
        useButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
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
        }
    }

    public void OnItemUsed()
    {
        if (item != null)
        {
            item.Use(); // we use the item
            OnItemRemoved(); // we remove the item for now // this is temporary
        }
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
