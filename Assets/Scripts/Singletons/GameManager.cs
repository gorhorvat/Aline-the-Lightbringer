using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    const int LumiberriesPerLife = 100;

    [SerializeField] PlayerData playerData;

    HashSet<CollectableType> pendingCollectables = new();
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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        HUDManager.Instance.UpdateLives(playerData.currentLives);
    }

    void ReloadLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        LoadLevel(levelName, Levels.GetLoadingMessage(levelName), false);
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
        LevelData level = GetLevelData(levelName);
        if (level != null) return level.isUnlocked;

        ZoneData zone = playerData.hub.GetZoneData(levelName);
        if (zone != null) return zone.isUnlocked;

        return true; // hub and other scenes always accessible
    }

    public LevelData GetLevelData(string levelName)
    {
        return playerData.hub.GetLevelData(levelName);
    }

    public void LoadLevel(string levelName, string message, bool clearPendingCollectables = true)
    {
        if (isLevelLoading) return;
        isLevelLoading = true;

        StartCoroutine(LoadLevelRoutine(levelName, message, clearPendingCollectables));
    }

    IEnumerator LoadLevelRoutine(string levelName, string message, bool clearPendingCollectables)
    {
        if (clearPendingCollectables)
        {
            pendingCollectables.Clear();
        }

        HUDManager.Instance.ShowLoadingScreen(message);

        // small delay to ensure loading screen is fully visible before unloading
        yield return new WaitForSecondsRealtime(0.1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
        operation.allowSceneActivation = false;

        float elapsed = 0f;

        while (elapsed < minimumLoadTime)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
        yield return operation;

        isLevelLoading = false;
        RefreshCollectablesOverlay();
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
        pendingCollectables.Add(type);
        RefreshCollectablesOverlay();
    }

    public bool IsLevelCollectableCollected(CollectableType type, string levelName)
    {
        if (pendingCollectables.Contains(type))
        {
            return true;
        }

        LevelData level = GetLevelData(levelName);

        if (level == null)
        {
            return false;
        }

        return level.IsCollected(type);
    }

    public void CommitLevelCollectables(string levelName)
    {
        LevelData level = GetLevelData(levelName);
        if (level == null)
        {
            return;
        }

        foreach (CollectableType type in pendingCollectables)
        {
            level.Collect(type);
        }

        pendingCollectables.Clear();
    }

    void RefreshCollectablesOverlay()
    {
        string levelName = SceneManager.GetActiveScene().name;
        LevelData level = GetLevelData(levelName);

        foreach (CollectableType type in Enum.GetValues(typeof(CollectableType)))
        {
            bool isPending = pendingCollectables.Contains(type);
            bool isCommitted = level != null && level.IsCollected(type);
            //Debug.Log($"{type} - pending: {isPending}, committed: {isCommitted}");
            HUDManager.Instance.UpdateCollectableIcon(type, isPending || isCommitted);
        }

        HUDManager.Instance.UpdateLives(playerData.currentLives);
        HUDManager.Instance.ShowCollectablesOverlay(playerData.currentLumiberries, playerData.totalLumiberries);
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