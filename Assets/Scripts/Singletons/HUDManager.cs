using System.Collections;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] CollectablesOverlay collectablesOverlayPrefab;
    [SerializeField] PauseMenu pauseMenuPrefab;
    [SerializeField] LoadingScreen loadingScreenPrefab;


    Coroutine hideCoroutine;
    CollectablesOverlay collectablesOverlay;
    LoadingScreen loadingScreen;
    float displayDuration = 5f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        collectablesOverlay = Instantiate(collectablesOverlayPrefab);
        loadingScreen = Instantiate(loadingScreenPrefab);
        Instantiate(pauseMenuPrefab);

        collectablesOverlay.Hide();
    }

    public void ShowCollectablesOverlay(int currentLumiberries, int totalLumiberries)
    {
        collectablesOverlay.UpdateLumiberries(currentLumiberries, totalLumiberries);

        // reset timer if already showing
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideCollectiblesOverlay());
    }

    public void UpdateCollectableIcon(CollectableType type, bool isActive)
    {
        collectablesOverlay.UpdateCollectableIcons(type, isActive);
    }

    IEnumerator HideCollectiblesOverlay()
    {
        yield return new WaitForSeconds(displayDuration);
        collectablesOverlay.Hide();
        hideCoroutine = null;
    }

    public void ShowLoadingScreen(string message) => loadingScreen.Show(message);
    public void HideLoadingScreen() => loadingScreen.Hide();

    public void UpdateLives(int lives)
    {
        collectablesOverlay.UpdateLives(lives);
    }
}