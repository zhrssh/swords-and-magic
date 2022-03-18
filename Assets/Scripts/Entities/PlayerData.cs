using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Player")]
public class PlayerData : ScriptableObject
{
    // Player Stats
    public PlayerClass playerClass;

    [Header("Stats")]
    public float maxHealth = 100.0f;
    public float maxArmor = 100.0f;


    [Header("Combat")]
    public float moveSpeed = 1f;
    public float attackRange = 0.1f;
}

public enum PlayerClass
{
    Knight,
    Rogue,
    Wizard,
    Heavy
}