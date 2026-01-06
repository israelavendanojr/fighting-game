using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    public void UpdateSeconds(int seconds)
    {
        if (timerText != null)
        {
            timerText.text = seconds.ToString();
        }
    }
}