using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] ParticleSystem portalParticles;
    [SerializeField] string targetLevel;

    private void Start()
    {
        if (!GameManager.Instance.IsLevelUnlocked(targetLevel))
        {
            DisablePortal();
        }
    }

    void DisablePortal()
    {
        portalParticles.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CommitLevelCollectables(SceneManager.GetActiveScene().name);
            GameManager.Instance.TryLoadLevel(targetLevel, Levels.GetLoadingMessage(targetLevel));
        }
    }
}