using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    const int LumiberriesPerLife = 100;

    [SerializeField] PlayerData playerData;
    float minimumLoadTime = 2f;
    bool isLevelLoading;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        HUDManager.Instance.UpdateLives(playerData.currentLives);
        HUDManager.Instance.ShowCollectablesOverlay(playerData.currentLumiberries, playerData.totalLumiberries);
    }

    void ReloadLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        LoadLevel(levelName, Levels.GetLoadingMessage(levelName));
    }

    public void TryLoadLevel(string levelName, string message)
    {
        if (IsLevelUnlocked(levelName))
        {
            LoadLevel(levelName, message);
        }
        else
        {
            Debug.Log($"{Levels.GetDisplayValue(levelName)} not unlocked yet!");
        }
    }

    public bool IsLevelUnlocked(string levelName)
    {
        LevelData level = playerData.hub.GetLevelData(levelName);
        if (level != null) return level.isUnlocked;

        ZoneData zone = playerData.hub.GetZoneData(levelName);
        if (zone != null) return zone.isUnlocked;

        return true; // hub and other scenes always accessible
    }

    public void LoadLevel(string levelName, string message)
    {
        if (isLevelLoading) return;
        isLevelLoading = true;

        StartCoroutine(LoadLevelRoutine(levelName, message));
    }

    IEnumerator LoadLevelRoutine(string levelName, string message)
    {
        HUDManager.Instance.ShowLoadingScreen(message);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
        operation.allowSceneActivation = false;

        float elapsed = 0f;

        while (elapsed < minimumLoadTime)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
        isLevelLoading = false;
        HUDManager.Instance.HideLoadingScreen();
    }

    public void AddLumiberry()
    {
        playerData.currentLumiberries++;
        playerData.totalLumiberries++;

        HUDManager.Instance.ShowCollectablesOverlay(playerData.currentLumiberries, playerData.totalLumiberries);

        if (playerData.currentLumiberries >= LumiberriesPerLife)
        {
            playerData.currentLumiberries -= LumiberriesPerLife;
            AddLife();
        }
    }

    void AddLife()
    {
        playerData.currentLives++;
        HUDManager.Instance.UpdateLives(playerData.currentLives);
    }

    public void LoseLife()
    {
        if (isLevelLoading) return;

        playerData.currentLives--;
        HUDManager.Instance.UpdateLives(playerData.currentLives);

        if (playerData.currentLives < 0)
        {
            QuitGame();
            return;
        }

        ReloadLevel();
    }

    public void CollectLevelCollectable(CollectableType type, string levelName)
    {
        LevelData level = playerData.hub.GetLevelData(levelName);
        if (level == null) return;

        level.Collect(type);
        //HUDManager.Instance.ShowCollectableCollected(type);
    }

    public bool IsLevelCollectableCollected(CollectableType type, string levelName)
    {
        LevelData level = playerData.hub.GetLevelData(levelName);
        if (level == null) return false;
        return level.IsCollected(type);
    }

    public void StartNewGame()
    {
        playerData.Reset();
        LoadLevel(Levels.LuminaGrove, Levels.GetLoadingMessage(Levels.LuminaGrove));
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }

    public bool IsInMainHub() => SceneManager.GetActiveScene().name == Levels.LuminaGrove;
}