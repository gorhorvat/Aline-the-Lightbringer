using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }

    public void StartNewGame()
    {
        GameManager.Instance.StartNewGamePrototype();
    }

    public void OpenOptions()
    {
        GameManager.Instance.OpenOptions();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}