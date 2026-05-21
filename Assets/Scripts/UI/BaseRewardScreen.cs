using TMPro;
using UnityEngine;

public abstract class BaseRewardScreen : MonoBehaviour
{
    [SerializeField] protected GameObject panel;
    [SerializeField] protected TMP_Text rewardText;
    [SerializeField] protected AudioClip rewardSfx;
    [SerializeField] protected float effectVolume;

    protected string levelToRedirect;

    protected void ShowScreen(string message, string targetLevel)
    {
        rewardText.text = message;
        panel.SetActive(true);
        AudioManager.Instance.PlaySfx(rewardSfx, transform.position, effectVolume);
        levelToRedirect = targetLevel;
    }

    public void OnConfirmClicked()
    {
        GameManager.Instance.ShowNextRewardPanel(levelToRedirect);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}