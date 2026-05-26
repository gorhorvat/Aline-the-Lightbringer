using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject returnToHubButton;

    PlayerInputActions inputActions;
    TMP_Text returnToHubText;
    bool isPaused;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        returnToHubText = returnToHubButton.GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        returnToHubText.text = $"Return to {GameManager.Instance.GetCurrentZone().Value}";
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Pause.performed += OnPause;
    }

    void OnDisable()
    {
        inputActions.Player.Pause.started -= OnPause;
        inputActions.Player.Disable();
    }

    void OnPause(InputAction.CallbackContext ctx)
    {
        Cursor.visible = true;

        if (GameManager.Instance.IsInMainMenu()) return;

        if (GameManager.Instance.IsOptionsVisible())
        {
            GameManager.Instance.CloseOptions();
            return;
        }

        if (isPaused) Resume();
        else Pause();
    }

    void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Cursor.visible = true;
        returnToHubButton.SetActive(!GameManager.Instance.IsInMainHub());
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void OpenOptions() => GameManager.Instance.OpenOptions();

    public void ReturnToHub()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        GameManager.Instance.ReturnToZone();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.QuitGame();
    }
}