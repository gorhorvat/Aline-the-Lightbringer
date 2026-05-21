using System.Collections;
using UnityEngine;

public class TransparentPlatform : MonoBehaviour
{
    [SerializeField] float toggleInterval = 2f; // seconds between toggles
    [SerializeField] float fadeSpeed = 1f; // how fast it fades

    [SerializeField] Material platformMaterial;

    MeshRenderer platformRenderer;
    Collider platformCollider;
    bool isSolid = true;

    void Start()
    {
        platformRenderer = GetComponent<MeshRenderer>();
        platformCollider = GetComponent<Collider>();
        platformRenderer.material = platformMaterial;

        StartCoroutine(ToggleLoop());
    }

    IEnumerator ToggleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(toggleInterval);
            yield return StartCoroutine(isSolid ? FadeOut() : FadeIn());
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            SetAlpha(Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        SetAlpha(0f);
        platformCollider.enabled = false;
        isSolid = false;
    }

    IEnumerator FadeIn()
    {
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
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            SetAlpha(Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        SetAlpha(1f);
    }

    void SetAlpha(float alpha)
    {
        Color c = platformRenderer.material.color;
        c.a = alpha;
        platformRenderer.material.color = c;
    }
}