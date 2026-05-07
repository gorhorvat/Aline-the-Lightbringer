using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] LoadingScreen loadingScreenPrefab;
    [SerializeField] float minimumLoadTime = 2f;

    LoadingScreen loadingScreen;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        loadingScreen = Instantiate(loadingScreenPrefab);

        DontDestroyOnLoad(gameObject);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName, string message)
    {
        StartCoroutine(LoadSceneRoutine(sceneName, message));
    }

    IEnumerator LoadSceneRoutine(string sceneName, string message)
    {
        loadingScreen.Show(message);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float elapsed = 0f;

        while (elapsed < minimumLoadTime || operation.progress < 0.9f)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
        loadingScreen.Hide();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}