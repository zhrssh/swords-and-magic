using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    // system
    Vector2 movement;
    bool isFacingRight = true;


    // references
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // if moving left
        if (movement.x < 0 && isFacingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            isFacingRight = false;
        }


        // if moving right
        if (movement.x > 0 && !isFacingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = true;
        }

        movement.Normalize();

        // Animation
        if (movement.magnitude > 0)
        {
            animator.SetBool("Is Moving", true);
        }
        else
        {
            animator.SetBool("Is Moving", false);
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
