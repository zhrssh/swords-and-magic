using System.Collections;
using UnityEngine;

public class Enemy : Damageable
{
    // Data Asset
    [SerializeField] EnemyData enemyData;

    // LayerMask
    [SerializeField] LayerMask whatIsPlayer;

    // Target
    Collider2D currentTarget;
    private bool isInRange = false;

    // Vectors
    Vector2 moveDelta;

    // Combat
    float lastAttack;

    protected override void Start()
    {
        base.Start();
        TargetingSystem.instance.AddEnemy(this);

        // overrides the current health and armor
        health = enemyData.maxHealth;
        armor = enemyData.maxArmor;
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            // The player is in range, attack
            if ((currentTarget.transform.position - transform.position).sqrMagnitude < enemyData.attackRange * enemyData.attackRange)
            {
                    isInRange = true;
                    HandleAttack();
            }

            // The player is not in range, chase
            else
            {
                isInRange = false;
                StartCoroutine(ChaseTarget());
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (currentTarget == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyData.detectionRange, whatIsPlayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == null)
                    return;

                if (colliders[i].GetComponent<Player>() != null)
                {
                    currentTarget = colliders[i];
                    colliders[i] = null; // clean the array
                    StartCoroutine(ChaseTarget());
                    break;
                }
            }
        }
    }

    private IEnumerator ChaseTarget()
    {
        while (currentTarget != null && !isInRange) // move to player when not in range
        {
            moveDelta = (currentTarget.transform.position - transform.position).normalized;
            rb.velocity = moveDelta * enemyData.moveSpeed;
            yield return null;
        }

        if (isInRange) // stop moving towards player when in range
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleAttack()
    {
        if (Time.time - lastAttack > enemyData.attackCooldown)
        {
            lastAttack = Time.time;
            Damage dmg = new Damage
            {
                origin = transform.position,
                damage = enemyData.attackDamage,
                pushForce = enemyData.pushForce
            };

            currentTarget.SendMessage("TakeDamage", dmg);
        }
    }

    protected override void HandleDeath()
    {
        TargetingSystem.instance.RemoveEnemy(this);
        base.HandleDeath();
    }
}
