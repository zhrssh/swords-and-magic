using UnityEngine;

public class Player : Entity
{
    PlayerController playerController;

    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();
    }

    public void SetTarget(Enemy enemy)
    {
        playerController.SetTarget(enemy);
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
        LevelManager.instance.DisplayDeathScreen();
    }
}
