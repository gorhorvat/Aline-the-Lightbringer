using UnityEngine;

public class TimeTrialPedestal : MonoBehaviour
{
    [SerializeField] AudioClip timeTrialClockSfx;
    [SerializeField] float effectVolume;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.IsTimeTrialActive())
        {
            AudioManager.Instance.PlaySfx(timeTrialClockSfx, transform.position, effectVolume);
            GameManager.Instance.StartTimeTrialMode();
        }
    }
}
