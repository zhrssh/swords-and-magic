using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumables : Item
{
    public float health;

    public override void Use(Player owner)
    {
        owner.Heal(health);
        RemoveItemFromInventory();
    }
}
