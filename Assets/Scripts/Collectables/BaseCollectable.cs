using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 90f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected();
            Destroy(gameObject);
        }
    }

    protected abstract void OnCollected();
}