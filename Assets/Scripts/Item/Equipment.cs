using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot;

    public float baseDamage;
    public float damagePercentReduction;
    public float pushForce;
    public float attackCooldown;

    public override void Use()
    {
        base.Use();

        // Equip the item
        if (EquipmentManager.instance.Equip(this)) // we don't need to instantiate because we already instantiated it in the inventory
            RemoveItemFromInventory(); // Remove from inventory
    }
}
public enum EquipmentSlot
{
    Weapon
}
