using System.Collections;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] CollectablesOverlay collectiblesOverlayPrefab;
    [SerializeField] PauseMenu pauseMenuPrefab;
    [SerializeField] LoadingScreen loadingScreenPrefab;


    Coroutine hideCoroutine;
    CollectablesOverlay collectiblesOverlay;
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

        collectiblesOverlay = Instantiate(collectiblesOverlayPrefab);
        loadingScreen = Instantiate(loadingScreenPrefab);
        Instantiate(pauseMenuPrefab);

        collectiblesOverlay.Hide();
    }

    public void ShowCollectablesOverlay(int currentLumiberries, int totalLumiberries)
    {
        collectiblesOverlay.UpdateLumiberries(currentLumiberries, totalLumiberries);

        // reset timer if already showing
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideCollectiblesOverlay());
    }

    IEnumerator HideCollectiblesOverlay()
    {
        yield return new WaitForSeconds(displayDuration);
        collectiblesOverlay.Hide();
        hideCoroutine = null;
    }

    public void ShowLoadingScreen(string message) => loadingScreen.Show(message);
    public void HideLoadingScreen() => loadingScreen.Hide();

    public void UpdateLives(int lives)
    {
        collectiblesOverlay.UpdateLives(lives);
    }
}