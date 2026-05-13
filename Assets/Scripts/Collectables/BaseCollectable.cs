using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 90f;

    protected AudioSource collectableCollectedSfx;

    private void Awake()
    {
        collectableCollectedSfx = GetComponent<AudioSource>();
    }

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
                AudioSource.PlayClipAtPoint(collectableCollectedSfx.clip, transform.position);
            }

            Destroy(gameObject);
        }
    }

    protected abstract void OnCollected();
}