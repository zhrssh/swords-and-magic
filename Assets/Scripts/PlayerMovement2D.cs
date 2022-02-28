using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    // system
    Vector2 movement;
    Vector2 lastMovement;
    bool isMoving = false;

    // interactables
    public Object[] objects;

    // references
    [SerializeField] Joystick joystick;
    Rigidbody2D rb;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        objects = GameObject.FindObjectsOfType(typeof(Interactable));

        // Inputs and Movements
        MyInput();
        Move();

        // Animation
        Animate();
    }

    private void MyInput()
    {
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;

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

        // Refers to the direction of the player
        if (movement.x > 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (movement.x < 0)
        {
            transform.localScale = Vector2.one;
        }
    }

    private void Move()
    {
        if (rb)
        {
            rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        }
    }

    private void Animate()
    {
        if (animator)
        {
            animator.SetBool("IsMoving", isMoving);
            animator.SetFloat("xInput", Mathf.Abs(lastMovement.x));
            animator.SetFloat("yInput", lastMovement.y);
        }
    }

    public void OnMainButtonTouch()
    {
        // check if on interactable
        for (int i = 0; i < objects.Length; i++)
        {
            Interactable interactable = (Interactable)objects[i];
            if (interactable != null && ((transform.position - interactable.transform.position).magnitude < interactable.radius))
            {
                interactable.Interact();
                break; // we break to avoid picking up multiple items
            }
        }

        // if not we attack
    }
}
