using System.Collections;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] LumiberryOverlay lumiberryOverlayPrefab;

    Coroutine hideCoroutine;
    LumiberryOverlay lumiberryOverlay;
    float displayDuration = 5f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        lumiberryOverlay = Instantiate(lumiberryOverlayPrefab);

        lumiberryOverlay.Hide();
    }

    public void ShowLumiberryOverlay(int currentLumiberries, int totalLumiberries)
    {
        lumiberryOverlay.UpdateLumiberries(currentLumiberries, totalLumiberries);

        // reset timer if already showing
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideLumiberryOverlay());
    }

    IEnumerator HideLumiberryOverlay()
    {
        yield return new WaitForSeconds(displayDuration);
        lumiberryOverlay.Hide();
        hideCoroutine = null;
    }

    public void UpdateLives(int lives)
    {
        lumiberryOverlay.UpdateLives(lives);
    }
}