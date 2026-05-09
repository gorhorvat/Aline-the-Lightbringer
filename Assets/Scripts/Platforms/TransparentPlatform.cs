using System.Collections;
using UnityEngine;

public class TransparentPlatform : MonoBehaviour
{
    [SerializeField] float toggleInterval = 2f; // seconds between toggles
    [SerializeField] float fadeSpeed = 1f; // how fast it fades

    [Header("Materials")]
    [SerializeField] Material solidMaterial;
    [SerializeField] Material transparentMaterial;

    private MeshRenderer platformRenderer;
    private Collider platformCollider;
    private bool isSolid = true;

    void Start()
    {
        platformRenderer = GetComponent<MeshRenderer>();
        platformCollider = GetComponent<Collider>();

        StartCoroutine(ToggleLoop());
    }

    private IEnumerator ToggleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(toggleInterval);
            yield return StartCoroutine(isSolid ? FadeOut() : FadeIn());
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        Color solidColor = solidMaterial.color;
        Color transparentColor = transparentMaterial.color;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            platformRenderer.material.color = Color.Lerp(solidColor, transparentColor, t);
            yield return null;
        }

        platformRenderer.material = transparentMaterial;
        platformCollider.enabled = false;
        isSolid = false;
    }

    private IEnumerator FadeIn()
    {
        // Check if player is inside before becoming solid
        Collider[] overlapping = Physics.OverlapBox(
            transform.position,
            transform.localScale / 2,
            transform.rotation,
            LayerMask.GetMask("Player")
        );

        if (overlapping.Length > 0)
        {
            GameManager.Instance.LoseLife();
            yield break;
        }

        platformCollider.enabled = true;
        isSolid = true;

        float t = 0f;
        Color solidColor = solidMaterial.color;
        Color transparentColor = transparentMaterial.color;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            platformRenderer.material.color = Color.Lerp(transparentColor, solidColor, t);
            yield return null;
        }

        platformRenderer.material = solidMaterial;
    }
}