using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] bool canRespawn = true;
    [SerializeField] float timeBeforeCrumble = 2f;
    [SerializeField] float shakeIntensity = 0.05f;
    [SerializeField] float shakeSpeed = 20f;
    [SerializeField] float respawnDelay = 10f;

    private Vector3 startPosition;
    private Rigidbody rb;
    private Collider platformCollider;
    private MeshRenderer platformRenderer;
    private bool isCrumbling;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        if (canRespawn)
        {
            platformCollider = GetComponent<Collider>();
            platformRenderer = GetComponent<MeshRenderer>();
        }
    }

    public void OnPlayerLand()
    {
        if (isCrumbling) return;

        isCrumbling = true;
        StartCoroutine(CrumbleSequence());
    }

    private IEnumerator CrumbleSequence()
    {
        // Shake for X seconds before crumbling
        float elapsed = 0f;
        while (elapsed < timeBeforeCrumble)
        {
            float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
            float offsetZ = Mathf.Sin(Time.time * shakeSpeed * 1.3f) * shakeIntensity;
            transform.position = startPosition + new Vector3(offsetX, 0f, offsetZ);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset position before falling
        rb.MovePosition(startPosition);

        // Enable physics so the platform can fall
        rb.isKinematic = false;
        rb.useGravity = true;

        // Wait long enough to actually see the platform fall
        yield return new WaitForSeconds(3f);

        if (canRespawn)
        {
            // Hide platform
            platformRenderer.enabled = false;
            platformCollider.enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
            transform.position = startPosition;

            yield return new WaitForSeconds(respawnDelay);

            // Respawn platform
            platformRenderer.enabled = true;
            platformCollider.enabled = true;
            isCrumbling = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}