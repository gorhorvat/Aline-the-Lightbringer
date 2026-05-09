using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    const int LumiberriesPerLife = 100;

    [SerializeField] PlayerData playerData;
    [SerializeField] LoadingScreen loadingScreenPrefab;

    LoadingScreen loadingScreen;
    float minimumLoadTime = 2f;
    bool isSceneLoading;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        loadingScreen = Instantiate(loadingScreenPrefab);
    }

    private void Start()
    {
        HUDManager.Instance.UpdateLives(playerData.CurrentLives);
        HUDManager.Instance.ShowLumiberryOverlay(playerData.CurrentLumiberries, playerData.TotalLumiberries);
    }

    void ReloadScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        LoadScene(sceneName, $"Loading {sceneName}");
    }

    public void LoadScene(string sceneName, string message)
    {
        if (isSceneLoading) return;
        isSceneLoading = true;

        StartCoroutine(LoadSceneRoutine(sceneName, message));
    }

    IEnumerator LoadSceneRoutine(string sceneName, string message)
    {
        loadingScreen.Show(message);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float elapsed = 0f;

        while (elapsed < minimumLoadTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
        isSceneLoading = false;
        loadingScreen.Hide();
    }

    public void AddLumiberry()
    {
        playerData.CurrentLumiberries++;
        playerData.TotalLumiberries++;

        HUDManager.Instance.ShowLumiberryOverlay(playerData.CurrentLumiberries, playerData.TotalLumiberries);

        if (playerData.CurrentLumiberries >= LumiberriesPerLife)
        {
            playerData.CurrentLumiberries -= LumiberriesPerLife;
            AddLife();
        }
    }

    void AddLife()
    {
        playerData.CurrentLives++;
        HUDManager.Instance.UpdateLives(playerData.CurrentLives);
    }

    public void LoseLife()
    {
        if (isSceneLoading) return;

        playerData.CurrentLives--;
        HUDManager.Instance.UpdateLives(playerData.CurrentLives);

        if (playerData.CurrentLives < 0)
        {
            QuitGame();
            return;
        }

        ReloadScene();
    }

    public void CollectBeaconCrystal(string levelName)
    {
        if (!IsCrystalCollected(levelName))
        {
            playerData.collectedBeaconCrystals.Add(levelName);
            Debug.Log(playerData.collectedBeaconCrystals[0]);
            // do we need to show some kind of overlay for this?
            // or not, we show it in zone hub and in "compendium"
        }
    }

    public bool IsCrystalCollected(string levelName) => playerData.IsBeaconCrystalCollected(levelName);

    public void StartNewGame()
    {
        playerData.Reset();
        LoadScene("FirstLevel", "Starting new game!");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}