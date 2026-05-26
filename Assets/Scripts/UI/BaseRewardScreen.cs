using TMPro;
using UnityEngine;

public abstract class BaseRewardScreen : MonoBehaviour
{
    [SerializeField] protected GameObject panel;
    [SerializeField] protected TMP_Text rewardText;
    [SerializeField] protected AudioClip rewardSfx;
    [SerializeField] protected float effectVolume;

    protected void ShowScreen(string message)
    {
        rewardText.text = message;
        panel.SetActive(true);
        Cursor.visible = true;
        AudioManager.Instance.PlaySfx(rewardSfx, transform.position, effectVolume);
    }

    public void OnConfirmClicked()
    {
        Hide();
        GameManager.Instance.ShowNextRewardPanel();
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}