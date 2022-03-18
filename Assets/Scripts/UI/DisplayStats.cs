using UnityEngine;
using TMPro;

public class DisplayStats : MonoBehaviour
{
    // Text References
    [Header("References")]
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI armor;
    [SerializeField] private Player player;

    // System
    private float healthAmount;
    private float armorAmount;

    private void Update()
    {
        if (player != null)
        {
            health.text = player.GetHealth().ToString();
            armor.text = player.GetArmor().ToString();
        }
    }
}
