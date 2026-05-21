using TMPro;
using UnityEngine;

public abstract class BaseRewardScreen : MonoBehaviour
{
    [SerializeField] protected GameObject panel;
    [SerializeField] protected TMP_Text rewardText;

    protected string levelToRedirect;

    protected void ShowScreen(string message, string targetLevel)
    {
        rewardText.text = message;
        panel.SetActive(true);
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