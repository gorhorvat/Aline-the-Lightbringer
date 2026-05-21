using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 90f;
    [SerializeField] protected GameObject collectedVFXPrefab;
    [SerializeField] protected AudioClip collectableCollectedSfx;
    [SerializeField] protected float effectVolume = 1f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected();

            if (collectableCollectedSfx != null)
            {
                AudioManager.Instance.PlaySfx(collectableCollectedSfx, transform.position, effectVolume);
            }

            Destroy(gameObject);
        }
    }

    protected virtual void OnCollected()
    {
        if (collectedVFXPrefab != null)
        {
            GameObject vfx = Instantiate(collectedVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }

    }
}