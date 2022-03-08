using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    // Systems
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float maxArmor = 100.0f;
    [SerializeField] private float pushRecoverySpeed = 0.2f;

    // private fields
    private float health;
    private float armor;

    // Immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    // Push Force
    protected Vector3 pushDirection;

    // References
    private Rigidbody2D rb;

    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;
        armor = maxArmor;
    }

    private void Update()
    {
        // Clamping
        health = Mathf.Clamp(health, 0.0f, maxHealth);
        armor = Mathf.Clamp(armor, 0.0f, maxArmor);
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
                }
                else if (armor <= 0f)
                {
                    health -= dmg.damage;
                }

                // Push Force
                pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

                if (health < 0f)
                {
                    isDead = true;
                    HandleDeath();
                }
            }
        }
    }

    protected virtual void Heal(float healReceived)
    {
        if (!isDead)
        {
            health += healReceived;
        }
    }

    protected virtual void RegenArmor(float armorReceived)
    {
        if (!isDead)
        {
            armor += armorReceived;
        }
    }

    protected virtual void HandleDeath()
    {
        if (!isDead)
        {
            Debug.Log("DEATH!");
        }
    }
}
