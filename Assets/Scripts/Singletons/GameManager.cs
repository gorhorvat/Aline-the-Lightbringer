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
    [SerializeField] AudioClip extraLifeSfx;
    [SerializeField] float effectVolume = 1f;

    HashSet<CollectableType> pendingCollectables = new();
    Queue<Action> rewardPanelQueue = new();
    float minimumLoadTime = 2f;
    bool isLevelLoading;
    bool isDeathless = true;
    bool isTimeTrialActive;
    float currentLevelTime;
    float finalLevelTime;

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

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        HUDManager.Instance.UpdateLives(playerData.currentLives);
    }

    void Update()
    {
        if (isTimeTrialActive)
        {
            currentLevelTime += Time.deltaTime;
            HUDManager.Instance.UpdateTimer(currentLevelTime);
        }
    }

    void ReloadLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        LoadLevel(levelName, Levels.GetLoadingMessage(levelName), false);
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
        EndTimeTrialMode();
        HUDManager.Instance.HideLoadingScreen();
    }

    void AddLife()
    {
        playerData.currentLives++;
        AudioManager.Instance.PlaySfx(extraLifeSfx, transform.position, effectVolume);
        HUDManager.Instance.UpdateLives(playerData.currentLives);
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

    public void AddLumiberry()
    {
        playerData.currentLumiberries++;
        playerData.totalLumiberries++;

        bool gainedLife = false;

        if (playerData.currentLumiberries >= LumiberriesPerLife)
        {
            playerData.currentLumiberries -= LumiberriesPerLife;
            gainedLife = true;
        }

        if (gainedLife)
        {
            AddLife();
        }

        HUDManager.Instance.ShowCollectablesOverlay(playerData.currentLumiberries, playerData.totalLumiberries);
    }

    public void LoseLife()
    {
        if (isLevelLoading) return;

        playerData.currentLives--;
        isDeathless = false;
        HUDManager.Instance.UpdateLives(playerData.currentLives);

        if (playerData.currentLives < 0)
        {
            QuitGame();
            return;
        }

        EndTimeTrialMode();

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

    public void OnPlayerReachedPortal(string targetLevel)
    {
        string levelName = SceneManager.GetActiveScene().name;
        LevelData level = GetLevelData(levelName);

        rewardPanelQueue.Clear();

        if (level != null)
        {
            bool isRadiantEmblemObtainable = isDeathless && level.hasRadiantEmblem && !level.radiantEmblemCollected;
            if (isRadiantEmblemObtainable)
            {
                pendingCollectables.Add(CollectableType.RadiantEmblem);
                rewardPanelQueue.Enqueue(() => HUDManager.Instance.ShowRadiantEmblemRewardPanel(levelName, targetLevel));

            }

            bool beatTargetTime = currentLevelTime <= level.chronoFeatherTargetTime;
            bool beatBestTime = level.chronoFeatherBestTime == 0f || currentLevelTime < level.chronoFeatherBestTime;
            bool isChronoFeatherObtainable = isTimeTrialActive && level.hasChronoFeather && beatTargetTime && beatBestTime;
            if (isChronoFeatherObtainable)
            {
                pendingCollectables.Add(CollectableType.ChronoFeather);
                finalLevelTime = currentLevelTime;
                level.chronoFeatherBestTime = finalLevelTime;
                rewardPanelQueue.Enqueue(() => HUDManager.Instance.ShowChronoFeatherRewardPanel(levelName, finalLevelTime, targetLevel));
            }
        }

        ShowNextRewardPanel(targetLevel);
    }

    public void ShowNextRewardPanel(string targetLevel)
    {
        if (rewardPanelQueue.Count > 0)
        {
            rewardPanelQueue.Dequeue().Invoke();
        }
        else
        {
            TriggerLevelTransition(targetLevel);
        }
    }

    public void TriggerLevelTransition(string targetLevel)
    {
        isDeathless = true;
        CommitLevelCollectables(SceneManager.GetActiveScene().name);
        TryLoadLevel(targetLevel, Levels.GetLoadingMessage(targetLevel));
    }

    public void ReturnToZone()
    {
        isDeathless = true;
        string currentZoneKey = GetCurrentZone().Key;
        TryLoadLevel(currentZoneKey, Levels.GetLoadingMessage(currentZoneKey));
    }

    public void StartTimeTrialMode()
    {
        isTimeTrialActive = true;
        currentLevelTime = 0f;

        HUDManager.Instance.ShowTimeTrialOverlay();
    }

    public void EndTimeTrialMode()
    {
        isTimeTrialActive = false;
        currentLevelTime = 0f;
        finalLevelTime = 0f;

        HUDManager.Instance.HideTimeTrialOverlay();
    }

    public KeyValuePair<string, string> GetCurrentZone()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        return Levels.GetZoneForLevel(currentLevel);
    }

    public bool IsInMainHub() => SceneManager.GetActiveScene().name == Levels.LuminaGrove;

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
}