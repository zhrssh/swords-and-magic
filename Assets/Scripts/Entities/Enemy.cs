using System;
using UnityEngine;

public class Enemy : Damageable
{
    // AI Stats
    [SerializeField] float detectionRange = 0.3f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackRange = 0.1f;
    [SerializeField] float pushForce = 2f;

    // LayerMask
    [SerializeField] LayerMask whatIsPlayer;

    // Target
    Collider2D currentTarget;

    // Vectors
    Vector2 moveDelta;


    protected override void Start()
    {
        base.Start();
        TargetingSystem.instance.AddEnemy(this);
    }

    private void Update()
    {
        if (currentTarget != null)
        {
           if ((currentTarget.transform.position - transform.position).magnitude < attackRange)
           {
               Damage dmg = new Damage
               {
                   origin = transform.position,
                   damage = attackDamage,
                   pushForce = this.pushForce
               };

               currentTarget.SendMessage("TakeDamage", dmg);
           }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, whatIsPlayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == null)
                return;

            if (colliders[i].GetComponent<Player>() != null)
            {
                currentTarget = colliders[i];
                colliders[i] = null; // clean the array
                break;
            }
        }

        ChaseTarget();
    }

    private void ChaseTarget()
    {
        if (currentTarget == null)
            return;

        moveDelta = (currentTarget.transform.position - transform.position).normalized;
        rb.velocity = moveDelta * moveSpeed;
    }

    protected override void HandleDeath()
    {
        TargetingSystem.instance.RemoveEnemy(this);
        base.HandleDeath();
    }
}
