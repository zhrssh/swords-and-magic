using System;
using UnityEngine;

public class PlayerWeapon : Collidable
{
    // Damage Struct
    public float baseDamage = 1;
    public float damagePercentReduction = 0;
    public float pushForce = 0.3f;
    public float attackCooldown= 1;

    // Equipment Manager
    EquipmentManager equipmentManager;

    // References
    SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider.isTrigger = true;
        boxCollider.autoTiling = true;

        equipmentManager = EquipmentManager.instance;
    }

    protected override void Update()
    {
        base.Update();

        for (int i = 0; i < equipmentManager.currentEquipment.Length; i++)
        {
            if (equipmentManager.currentEquipment[i] != null)
            {
                UpdateWeapon(equipmentManager.currentEquipment[i]);
            }
            else
            {
                UpdateWeapon(null);
            }
        }
    }

    private void UpdateWeapon(Equipment item)
    {
        if (item != null)
        {
            // Weapon's properties
            baseDamage = item.baseDamage;
            damagePercentReduction = item.damagePercentReduction;
            pushForce = item.pushForce;
            attackCooldown = item.attackCooldown;

            spriteRenderer.sprite = item.sprite;
        }
        else
        {
            // Default values if no weapon
            baseDamage = 1;
            damagePercentReduction = 0;
            pushForce = 0.3f;
            attackCooldown = 1;

            spriteRenderer.sprite = null;
        }
    }

    protected override void OnCollide(Collider2D collider2D)
    {
        base.OnCollide(collider2D);
        if (collider2D.tag == "Player")
            return; // we ignore player

        // Do damage
        Debug.Log("Damaging " + collider2D.name);
    }
}
