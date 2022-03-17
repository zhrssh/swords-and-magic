using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public delegate void OnEquipmentChange(Equipment newItem, Equipment oldItem);
    public OnEquipmentChange onEquipmentChange;

    public Equipment[] currentEquipment;
    Inventory inventory;

    private void Start()
    {
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];

        inventory = Inventory.instance;
    }

    public bool Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipmentSlot;
        Equipment oldItem = null;

        // if not holding two handed equipment
        // and there is item currently equipped
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];

            // this is to avoid deletion of item when inventory is full
            // if inventory is full we do nothing
            if (!inventory.Add(oldItem))
            {
                return false;
            }
        }

        // used to notify changes
        if (onEquipmentChange != null)
        {
            onEquipmentChange.Invoke(newItem, oldItem);
        }

        // after adding it back to the inventory, we swap it to the new item
        currentEquipment[slotIndex] = newItem;

        return true;
    }

    public void UnEquip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            if (!inventory.Add(oldItem))
            {
                // this is to avoid deletion of item when inventory is full
                return; // if we don't have enough space, we do nothing
            }

            // used to notify changes
            if (onEquipmentChange != null)
            {
                onEquipmentChange.Invoke(null, oldItem);
            }

            currentEquipment[slotIndex] = null;
        }
    }

    public void UnEquipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            UnEquip(i);
        }
    }

    private void Update()
    {
        
    }
}
