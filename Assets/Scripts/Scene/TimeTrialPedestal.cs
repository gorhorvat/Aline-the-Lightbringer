using UnityEngine;

public class TimeTrialPedestal : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.StartTimeTrialMode();
        }
    }
}
