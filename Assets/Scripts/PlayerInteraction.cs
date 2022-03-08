using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float range = 0.15f;
    [SerializeField] LayerMask whatIsInteractable;

    // Equipment
    PlayerWeapon weapon;

    // Combat
    private float lastAttack;
    private bool isAttacking;

    // Animation of attack varies within the weapon

    private void Start()
    {
        weapon = GetComponentInChildren<PlayerWeapon>();
    }

    private void Update()
    {
        // Combat
        HandleAttack();
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
            isAttacking = true;
        }
    }

    private void HandleAttack()
    {
        if (isAttacking)
        {
            if (Time.time - lastAttack > weapon.attackCooldown)
            {
                lastAttack = Time.time;
                Swing();
            }
        }
    }

    private void Swing()
    {
        Debug.Log("HIYAHHH! Swing not implemented yet");

        // Swing using the weapon's animation
        if(weapon.attackAnim != null)
            weapon.attackAnim.Play();

        // After the animation, we can swing
        isAttacking = false;
    }
}
