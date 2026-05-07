using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] string targetScene;
    [SerializeField] string loadingMessage = "Loading...";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.LoadScene(targetScene, loadingMessage);
    }
}