using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject returnToHubButton;
    [SerializeField] OptionsMenu optionsMenu;

    PlayerInputActions inputActions;
    bool isPaused;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Pause.started += OnPause;
    }

    void OnDisable()
    {
        inputActions.Player.Pause.started -= OnPause;
        inputActions.Player.Disable();
    }

    void OnPause(InputAction.CallbackContext ctx)
    {
        if (isPaused) Resume();
        else Pause();
    }

    void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        returnToHubButton.SetActive(!GameManager.Instance.IsInMainHub());
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenOptions()
    {
        optionsMenu.Show(AudioManager.Instance.audioSettings);
    }

    public void ReturnToHub()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadLevel(Levels.LuminaGrove, Levels.GetLoadingMessage(Levels.LuminaGrove));
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.QuitGame();
    }
}