using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    #region SINGLETON

    private static TargetingSystem _instance;
    public static TargetingSystem instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TargetingSystem>();
            }

            return _instance;
        }
    }

    #endregion

    private List<Enemy> enemies;
    private Enemy currentTarget;
    
    // Player
    [SerializeField] Player player;
    [SerializeField] float playerRange = 0.75f;

    // Camera
    [SerializeField] Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] float cameraThreshold;
    [SerializeField] Transform VCamFollowTarget;

    // Sprite
    // Target Indicator

    private void Start()
    {
        enemies = new List<Enemy>();
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (currentTarget == enemy)
        {
            // remove the focus on the dead target
            player.SetTarget(null);
            currentTarget = null; 
        }

        enemies.Remove(enemy);
    }


    private void Update()
    {
        FindNearestEnemy();
        HandleCameraPos();
    }

    private void FindNearestEnemy()
    {
        // Setting the target based on the nearest enemy on the player
        if (enemies.Count > 0)
        {
            foreach(Enemy enemy in enemies)
            {
                if (enemy == null || player == null) continue; // to avoid null reference
                if ((player.transform.position - enemy.transform.position).sqrMagnitude < playerRange * playerRange)
                {
                    // Set the current target to the nearest enemy to the player
                    if (currentTarget != null)
                    {
                        if ((player.transform.position - currentTarget.transform.position).sqrMagnitude > (player.transform.position - enemy.transform.position).sqrMagnitude)
                        {
                            player.SetTarget(enemy);
                            currentTarget = enemy;
                        }
                    }
                    else
                    {
                        player.SetTarget(enemy);
                        currentTarget = enemy;
                    }
                    break;
                }
                else
                {
                    player.SetTarget(null);
                    currentTarget = null;
                }
            }
        }
    }

    private void HandleCameraPos()
    {
        // To avoid accessing null references
        if (player == null)
        {
            return;
        }

        // Making the camera focus on both player and the target
        if (currentTarget != null)
        {
            Vector3 targetPos = (currentTarget.transform.position + player.transform.position) / 2f;
            
            targetPos.x = Mathf.Clamp(targetPos.x, -cameraThreshold + player.transform.position.x, cameraThreshold + player.transform.position.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -cameraThreshold + player.transform.position.y, cameraThreshold + player.transform.position.y);

            // clamp the camera between the player and the target
            VCamFollowTarget.transform.position = targetPos; 
        }
        else
        {
            // if there is no target, we follow the player
            VCamFollowTarget.transform.position = player.transform.position; 
        }
    }
}
