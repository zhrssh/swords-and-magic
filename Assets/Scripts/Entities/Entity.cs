using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Systems
    protected float maxHealth = 100.0f;
    protected float maxArmor = 100.0f;
    protected float health;
    public float GetHealth() { return health; }
    protected float armor;
    public float GetArmor() { return armor; }

    // Immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    // Push Force
    protected Vector3 pushDirection;
    private float pushRecoverySpeed = 0.2f;
    private float lastPush;

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

                // Push Force is not yet implemented
                pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
                StartCoroutine(Knockback(pushDirection));
            }
        }

        if (health <= 0f)
        {
            isDead = true;
            HandleDeath();
        }
    }

    private IEnumerator Knockback(Vector3 pushDirection)
    {
        lastPush = 0;
        while (lastPush < immuneTime)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(pushDirection, ForceMode2D.Impulse);
            lastPush += pushRecoverySpeed;
            yield return null;
        }

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    protected virtual void FixedUpdate()
    {
        // meant to be overriden
    }

    public virtual void Heal(float healReceived)
    {
        if (!isDead)
        {
            health += healReceived;
            health = Mathf.Clamp(health, 0.0f, maxHealth);
        }
    }

    public virtual void RegenArmor(float armorReceived)
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
            gameObject.SetActive(false);
        }
    }
}
