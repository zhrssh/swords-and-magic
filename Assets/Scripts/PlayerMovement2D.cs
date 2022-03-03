using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float range = 0.1f;
    public Vector3 handOffset = Vector3.zero;

    // system
    Vector2 movement;
    Vector2 lastMovement;

    bool isMoving = false;
    bool isAiming = false;

    Vector2 aim;

    // interactables
    public Object[] objects;

    // references
    [SerializeField] Joystick moveJoystick;
    [SerializeField] Joystick aimJoystick;
    [SerializeField] GameObject hand;
    Rigidbody2D rb;
    Animator animator;

    private void Start()
    {
        hand.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        objects = GameObject.FindObjectsOfType(typeof(Interactable));

        // Inputs and Movements
        MyInput();
        MyAim();
        Move();

        // Animation
        Animate();
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

    private void MyAim()
    {
        // Aiming System
        aim.x = aimJoystick.Horizontal;
        aim.y = aimJoystick.Vertical;

        Vector3 direction = new Vector3(aim.x * range, aim.y * range);
        hand.transform.localPosition = handOffset + direction;

        if (aim.magnitude > 0f)
        {
            isAiming = true;
            hand.SetActive(true);
        }
        else
        {
            isAiming = false;
            hand.SetActive(false);
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

            animator.SetFloat("xInput", (isAiming) ? aim.x : lastMovement.x);
            animator.SetFloat("yInput", (isAiming) ? aim.y : lastMovement.y);
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

    public bool IsAiming()
    {
        return isAiming;
    }
}
