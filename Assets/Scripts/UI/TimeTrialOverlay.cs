using UnityEngine;
using TMPro;

public class TimeTrialOverlay : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text timerText;

    public void Show()
    {
        panel.SetActive(true);
    }

    public void UpdateTimer(float currentTime)
    {
        timerText.text = Utils.FormatTime(currentTime);
    }

    public void Hide() => panel.SetActive(false);
}