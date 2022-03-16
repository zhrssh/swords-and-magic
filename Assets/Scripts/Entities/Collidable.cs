using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    protected BoxCollider2D boxCollider;
    private List<Collider2D> hits;

    protected virtual void Start()
    {
        hits = new List<Collider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        boxCollider.OverlapCollider(contactFilter, hits);

        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i] != null)
            {
                OnCollide(hits[i]);
                hits[i] = null; // we clean the array;
            }
        }
    }

    protected virtual void OnCollide(Collider2D collider2D)
    {
        // Do something
    }
}
