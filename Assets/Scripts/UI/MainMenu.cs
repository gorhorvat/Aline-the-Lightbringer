using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
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