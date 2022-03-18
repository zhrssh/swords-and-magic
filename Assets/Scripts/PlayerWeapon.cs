using System;
using UnityEngine;

public class PlayerWeapon : Collidable
{
    // Damage Struct
    [SerializeField] float baseDamage = 1;
    [SerializeField] float pushForce = 0.3f;
    public float attackCooldown = 1;
    public bool canDamage = false;

    // Equipment Manager
    EquipmentManager equipmentManager;

    // References
    SpriteRenderer spriteRenderer;

    // Default Weapon and Animation
    [SerializeField] private Sprite fist;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider.isTrigger = true;
        boxCollider.autoTiling = true;

        equipmentManager = EquipmentManager.instance;

        UpdateWeaponProperties(null);
    }

    protected override void Update()
    {
        base.Update();

        // Updates the weapon based on the equipped weapon
        for (int i = 0; i < equipmentManager.currentEquipment.Length; i++)
        {
            if (equipmentManager.currentEquipment[i] != null)
            {
                UpdateWeaponProperties(equipmentManager.currentEquipment[i]);
            }
            else
            {
                UpdateWeaponProperties(null);
            }
        }
    }

    private void UpdateWeaponProperties(Equipment item)
    {
        if (item != null)
        {
            // Weapon's properties
            baseDamage = item.baseDamage;
            pushForce = item.pushForce;
            attackCooldown = item.attackCooldown;

            // Sprites and Colliders
            spriteRenderer.sprite = item.sprite;
            boxCollider.size = item.sprite.bounds.size;
        }
        else
        {
            // Default values if no weapon
            baseDamage = 1;
            pushForce = 1;
            attackCooldown = 1;

            // Sprites and Colliders
            spriteRenderer.sprite = fist;
            boxCollider.size = fist.bounds.size;
        }
    }

    protected override void OnCollide(Collider2D collider2D)
    {
        if (canDamage)
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
}
