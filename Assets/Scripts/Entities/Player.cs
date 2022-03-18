using UnityEngine;

public class Player : Damageable
{
    PlayerMovement2D playerMovement2D;

    protected override void Start()
    {
        base.Start();
        playerMovement2D = GetComponent<PlayerMovement2D>();
    }

    public void SetTarget(Enemy enemy)
    {
        playerMovement2D.SetTarget(enemy);
    }
}
