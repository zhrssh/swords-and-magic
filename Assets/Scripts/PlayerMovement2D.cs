using System;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float range = 0.1f;

    // system
    Vector2 movement;
    Vector2 lastMovement;

    bool isMoving = false;

    // references
    [SerializeField] Joystick moveJoystick;
    Rigidbody2D rb;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Inputs and Movements
        MyInput();
        Move();

        // Animation
        AnimateMovement();
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
    }

    private void Move()
    {
        if (rb)
        {
            rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        }
    }
}