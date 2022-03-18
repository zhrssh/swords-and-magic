using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Data
    [Header("Player Data")]
    [SerializeField] private PlayerData playerData;

    // Interaction
    [Header("Interaction")]
    [SerializeField] LayerMask whatIsInteractable;
    private float lastAttack;

    // Equipment
    PlayerWeapon weapon;
    EquipmentManager equipmentManager;

    // Movements
    private Vector2 movement;
    private Vector2 lastMovement;
    bool isMoving = false;

    // Controller
    [Header("Movement UI Controller")]
    [SerializeField] Joystick moveJoystick;

    // Animation
    [Header("Animation")]
    [SerializeField] AnimationClip defaultAttack;
    [SerializeField] AnimationClip meleeAttack;
    [SerializeField] AnimationClip staffAttack;
    const string TRIGGER_ATTACK = "Attack";
    AnimatorOverrideController overrideController;
    private Animator animator;
    
    // Rigid Body
    private Rigidbody2D rb;

    // Targeting System
    private Enemy currentTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        weapon = GetComponentInChildren<PlayerWeapon>();
        equipmentManager = EquipmentManager.instance;
        animator = GetComponent<Animator>();

        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }

    private void Update()
    {
        // Inputs and Movements
        MyInput();

        // Animate
        HandleEquipmentAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void MyInput()
    {
        // Movement
        movement.x = moveJoystick.Horizontal;
        movement.y = moveJoystick.Vertical;
        movement.Normalize();

        // checks if the player is moving
        if (movement.magnitude > 0f)
        {
            // this function remembers the last direction the player was facing
            lastMovement = movement;
            isMoving = true;
        }
        else
            isMoving = false;

        HandleOrientation();
    }

    private void HandleEquipmentAnimation()
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

    private void HandleOrientation()
    {
        // Flip the player on the y axis
        if (currentTarget == null)
        {
            if (lastMovement.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            if (transform.position.x < currentTarget.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void Move()
    {
        if (rb)
        {
            rb.velocity = movement * playerData.moveSpeed;
        }
    }

    private void AnimateMovement()
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);

            animator.SetFloat("xInput", lastMovement.x);
            animator.SetFloat("yInput", lastMovement.y);
        }
    }

    private void Attack()
    {
        // Play Animation
        animator.SetTrigger(TRIGGER_ATTACK);
    }

    //****** Public Functions ******//
    public void SetTarget(Enemy enemy)
    {
        currentTarget = enemy;
    }

    public void OnAttackButtonPressed()
    {
        if (Time.time - lastAttack > weapon.attackCooldown)
        {
            lastAttack = Time.time;
            Attack();
        }
    }

    public void OnPickUpButtonPressed()
    {
        // check if on interactable
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, playerData.attackRange, whatIsInteractable);
        
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    colliders[i].GetComponent<Interactable>().Interact();
                    colliders[i] = null; // we clean the array
                    break;
                }
            }
        }
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