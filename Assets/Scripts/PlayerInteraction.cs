using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float range = 0.15f;
    [SerializeField] LayerMask whatIsInteractable;

    // Equipment
    PlayerWeapon weapon;
    EquipmentManager equipmentManager;

    // Animation
    const string TRIGGER_ATTACK = "Attack";
    Animator animator;
    AnimatorOverrideController overrideController;

    // Animation Clips
    [SerializeField] AnimationClip defaultAttack;
    [SerializeField] AnimationClip meleeAttack;
    [SerializeField] AnimationClip staffAttack;

    // Combat
    private float lastAttack;

    private void Start()
    {
        weapon = GetComponentInChildren<PlayerWeapon>();
        equipmentManager = EquipmentManager.instance;
        animator = GetComponent<Animator>();

        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }

    private void Update()
    {
        // Check Current Weapon
        Equipment equipment = CheckCurrentWeapon();

        if (equipment != null)
        {
            switch (equipment.equipmentType)
            {
                case EquipmentType.Melee:
                    SetAnimation(meleeAttack);
                    break;
                case EquipmentType.Staff:
                    SetAnimation(staffAttack);
                    break;
                default:
                    SetAnimation(defaultAttack);
                    break;
            }
        }
    }

    private void SetAnimation(AnimationClip anim)
    {
        overrideController["Default Attack"] = anim;
    }

    private Equipment CheckCurrentWeapon()
    {
        return equipmentManager.currentEquipment[0];
    }

    public void OnMainButtonTouch()
    {
        // check if on interactable
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, whatIsInteractable);
        
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    colliders[i].GetComponent<Interactable>().Interact();
                    break;
                }
            }
        }

        // if not we attack
        else
        {
            if (Time.time - lastAttack > weapon.attackCooldown)
            {
                lastAttack = Time.time;
                Attack();
            }
        }
    }

    private void Attack()
    {
        // Play Animation
        animator.SetTrigger(TRIGGER_ATTACK);
    }

    public void AnimationEnter()
    {
        weapon.canDamage = true;
    }

    public void AnimationEnded()
    {
        weapon.canDamage = false;
    }
}
