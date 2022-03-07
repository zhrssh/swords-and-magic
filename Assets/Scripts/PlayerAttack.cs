using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float range = 0.15f;
    [SerializeField] LayerMask whatIsInteractable;

    private void Start()
    {
        
    }

    public void OnMainButtonTouch()
    {
        // check if on interactable
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, whatIsInteractable);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null)
            {
                colliders[i].GetComponent<Interactable>().Interact();
                break;
            }
        }
        // if not we attack
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
