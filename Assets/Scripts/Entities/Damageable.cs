using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    // Systems
    [SerializeField] protected float maxHealth = 100.0f;
    [SerializeField] protected float maxArmor = 100.0f;
    [SerializeField] protected float pushRecoverySpeed = 0.2f;

    // private fields
    private float health;
    private float armor;

    // Immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    // Push Force
    protected Vector3 pushDirection;

    // References
    protected Rigidbody2D rb;

    private bool isDead = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        health = maxHealth;
        armor = maxArmor;
    }

    public bool IsDead()
    {
        return isDead;
    }

    protected virtual void TakeDamage(Damage dmg)
    {
        // Immunity cooldown to avoid spamming
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;

            // Take Damage and Push Force
            if (!isDead)
            {
                if (armor > 0f)
                {
                    armor -= dmg.damage;
                    armor = Mathf.Clamp(armor, 0.0f, maxArmor);
                }
                else if (armor <= 0f)
                {
                    health -= dmg.damage;
                    health = Mathf.Clamp(health, 0.0f, maxHealth);
                }

                // Push Force
                pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce * 100f;
            }
        }

        if (health <= 0f)
        {
            isDead = true;
            HandleDeath();
        }
    }

    protected virtual void FixedUpdate()
    {
        // Handles push direction
        if (pushDirection.magnitude > 0)
        {
            rb.AddForce(pushDirection, ForceMode2D.Impulse);
            pushDirection = Vector2.zero;
        }
    }

    protected virtual void Heal(float healReceived)
    {
        if (!isDead)
        {
            health += healReceived;
            health = Mathf.Clamp(health, 0.0f, maxHealth);
        }
    }

    protected virtual void RegenArmor(float armorReceived)
    {
        if (!isDead)
        {
            armor += armorReceived;
            armor = Mathf.Clamp(armor, 0.0f, maxArmor);
        }
    }

    protected virtual void HandleDeath()
    {
        if (isDead)
        {
            Debug.Log("DEATH!");
            Destroy(gameObject);
        }
    }
}
