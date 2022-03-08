using System;
using UnityEngine;

public class PlayerWeapon : Collidable
{
    // Damage Struct
    [SerializeField] float baseDamage = 1;
    [SerializeField] float damagePercentReduction = 0;
    [SerializeField] float pushForce = 0.3f;
    public float attackCooldown = 1;

    // Attack Animation
    public Animation attackAnim;

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

        // Updates the weapon based on the equipped weapon
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
            attackAnim = item.attackAnim;
        }
        else
        {
            // Default values if no weapon
            baseDamage = 1;
            damagePercentReduction = 0;
            pushForce = 0.3f;
            attackCooldown = 1;

            spriteRenderer.sprite = null;
            attackAnim = null;
        }
    }

    protected override void OnCollide(Collider2D collider2D)
    {
        if (collider2D.name == "Player") // temporary condition
            return; // we ignore player

        // Do damage
        Damage dmg = new Damage
        {
            origin = transform.position,
            damage = baseDamage,
            pushForce = this.pushForce
        };

        collider2D.SendMessage("TakeDamage", dmg);
    }
}
