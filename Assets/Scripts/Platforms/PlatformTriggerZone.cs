using UnityEngine;

public class PlatformTriggerZone : MonoBehaviour
{
    private BaseMovingPlatform platform;

    void Awake()
    {
        platform = GetComponentInParent<BaseMovingPlatform>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(platform.transform);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
