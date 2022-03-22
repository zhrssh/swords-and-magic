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

    private void Update()
    {
        if (player != null)
        {
            health.text = player.GetHealth().ToString();
            armor.text = player.GetArmor().ToString();
        }

        if (WaveManager.instance != null)
        {
            time.text = (WaveManager.instance.waveCountdown <= 0) ? "0" : Mathf.FloorToInt(WaveManager.instance.waveCountdown).ToString();
            wave.text = WaveManager.instance.waveSurvived.ToString();
        }
    }
}
