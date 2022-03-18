using UnityEngine;

public class Player : Damageable
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
}
