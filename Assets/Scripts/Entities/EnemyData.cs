using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Enemy")]
public class EnemyData : ScriptableObject
{
    // AI Stats
    public EnemyType enemyType = EnemyType.Normal;

    [Header("Stats")]
    public float maxHealth = 100.0f;
    public float maxArmor = 100.0f;


    [Header("Combat")]
    public float detectionRange = 0.3f;
    public float moveSpeed = 1f;
    public float attackDamage = 10f;
    public float attackRange = 0.1f;
    public float pushForce = 2f;
}

public enum EnemyType
{
    Fast,
    Normal,
    Heavy,
    Boss
}