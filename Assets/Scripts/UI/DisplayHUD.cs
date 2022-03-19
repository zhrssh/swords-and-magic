using UnityEngine;
using TMPro;

public class DisplayHUD : MonoBehaviour
{
    // Text References
    [Header("References")]
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI armor;
    [SerializeField] private TextMeshProUGUI wave;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private Player player;


    // Manager References
    WaveManager waveManager;

    private void Start()
    {
        waveManager = WaveManager.instance;
    }

    private void Update()
    {
        if (player != null)
        {
            health.text = player.GetHealth().ToString();
            armor.text = player.GetArmor().ToString();
        }

        if (waveManager != null)
        {
            wave.text = (waveManager.GetWaveNumber() - 1).ToString();
            time.text = (Mathf.FloorToInt(waveManager.GetWaveCountdown()).ToString());
        }
    }
}
