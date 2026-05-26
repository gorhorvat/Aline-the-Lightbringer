using System.Collections;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] CollectablesOverlay collectablesOverlayPrefab;
    [SerializeField] TimeTrialOverlay timeTrialOverlayPrefab;
    [SerializeField] PauseMenu pauseMenuPrefab;
    [SerializeField] LoadingScreen loadingScreenPrefab;
    [SerializeField] RadiantEmblemRewardScreen radiantEmblemRewardScreenPrefab;
    [SerializeField] ChronoFeatherRewardScreen chronoFeatherRewardScreenPrefab;

    Coroutine hideCoroutine;
    CollectablesOverlay collectablesOverlay;
    TimeTrialOverlay timeTrialOverlay;
    PauseMenu pauseMenu;
    OptionsMenu optionsMenu;
    LoadingScreen loadingScreen;
    RadiantEmblemRewardScreen radiantEmblemRewardScreen;
    ChronoFeatherRewardScreen chronoFeatherRewardScreen;
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
        timeTrialOverlay = Instantiate(timeTrialOverlayPrefab);
        loadingScreen = Instantiate(loadingScreenPrefab);
        radiantEmblemRewardScreen = Instantiate(radiantEmblemRewardScreenPrefab);
        chronoFeatherRewardScreen = Instantiate(chronoFeatherRewardScreenPrefab);
        pauseMenu = Instantiate(pauseMenuPrefab);
        optionsMenu = pauseMenu.GetComponentInChildren<OptionsMenu>(true);
    }

    public void ShowCollectablesOverlay(int currentLumiberries, int totalLumiberries)
    {
        collectablesOverlay.UpdateLumiberries(currentLumiberries, totalLumiberries);

        // reset timer if already showing
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideCollectiblesOverlay());
    }

    public void UpdateCollectableIcon(CollectableType type, bool isActive) => collectablesOverlay.UpdateCollectableIcon(type, isActive);

    IEnumerator HideCollectiblesOverlay()
    {
        yield return new WaitForSeconds(displayDuration);
        collectablesOverlay.Hide();
        hideCoroutine = null;
    }

    public void ShowTimeTrialOverlay() => timeTrialOverlay.Show();

    public void HideTimeTrialOverlay() => timeTrialOverlay.Hide();

    public void UpdateTimer(float currentTime) => timeTrialOverlay.UpdateTimer(currentTime);

    public void ShowRadiantEmblemRewardPanel(string levelName) => radiantEmblemRewardScreen.Show(levelName);

    public void CloseRadiantEmblemRewardPanel() => radiantEmblemRewardScreen.Hide();

    public void ShowChronoFeatherRewardPanel(string levelName, float finalTime) => chronoFeatherRewardScreen.Show(levelName, finalTime);

    public void CloseChronoFeatherRewardPanel() => chronoFeatherRewardScreen.Hide();

    public void ShowLoadingScreen(string message) => loadingScreen.Show(message);

    public void HideLoadingScreen() => loadingScreen.Hide();

    public void UpdateLives(int lives) => collectablesOverlay.UpdateLives(lives);

    public bool IsOptionsVisible => optionsMenu.IsVisible;

    public void OpenOptions() => optionsMenu.Show(AudioManager.Instance.audioSettings);
    
    public void CloseOptions() => optionsMenu.Hide();
}