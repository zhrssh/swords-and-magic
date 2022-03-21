using System.Collections;
using UnityEngine;

public class Enemy : Entity, IObjectPooled
{
    // Data Asset
    [SerializeField] EnemyData enemyData;

    // LayerMask
    [SerializeField] LayerMask whatIsPlayer;

    // States
    private enum EnemyState { Idle, Chasing, Attacking };
    private EnemyState state;

    // Combat
    private float attackPrepTime = 1f;

    // Target
    Collider2D currentTarget;
    private bool isInRange = false;

    // Vectors
    Vector2 moveDelta;

    // Combat
    float lastAttack;

    // Sprite Renderer
    SpriteRenderer sprite;

    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();

        // overrides the current health and armor
        health = enemyData.maxHealth;
        armor = enemyData.maxArmor;
    }

    public void OnObjectSpawned(Vector3 position, Quaternion rotation)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.localRotation = rotation;

        // fixes enemy not chasing after being recycled
        state = EnemyState.Idle;

        // fix for falling enemies out of map
        // fixes the null reference
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }


        // overrides the current health and armor
        health = enemyData.maxHealth;
        armor = enemyData.maxArmor;

        // we replace the physics overlap all for better performance
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerObj.TryGetComponent<Collider2D>(out currentTarget);

        // Targeting System
        TargetingSystem.instance.AddEnemy(this);
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            // The player is in range, attack
            if ((currentTarget.transform.position - transform.position).sqrMagnitude < enemyData.attackRange * enemyData.attackRange)
            {
                isInRange = true;
                
                if (state != EnemyState.Attacking)
                    StartCoroutine(HandleAttack());
            }

            // The player is not in range, chase
            else
            {
                isInRange = false;
                
                if (state != EnemyState.Chasing)
                    StartCoroutine(ChaseTarget());
            }
        }
    }

    private IEnumerator ChaseTarget()
    {
        state = EnemyState.Chasing;

        while (currentTarget != null && !isInRange) // move to player when not in range
        {
            moveDelta = (currentTarget.transform.position - transform.position).normalized;
            rb.velocity = moveDelta * enemyData.moveSpeed;
            yield return null;
        }
    }

    private IEnumerator HandleAttack()
    {
        // Play Attack Preparation
        state = EnemyState.Attacking;

        // Temporary  Indicator, will remove later
        sprite.color = Color.red;
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(attackPrepTime);

        sprite.color = Color.white;

        if (Time.time - lastAttack > enemyData.attackCooldown)
        {
            lastAttack = Time.time;
            Damage dmg = new Damage
            {
                origin = transform.position,
                damage = enemyData.attackDamage,
                pushForce = enemyData.pushForce
            };

            // We only damage if the player is in range
            if (currentTarget.gameObject.activeSelf && isInRange)
                currentTarget.SendMessage("TakeDamage", dmg);
        }

        yield break;
    }

    protected override void HandleDeath()
    {
        TargetingSystem.instance.RemoveEnemy(this);
        gameObject.SetActive(false);
    }
}
