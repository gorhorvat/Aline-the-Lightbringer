using UnityEngine;

public class PortalInfo : MonoBehaviour
{
    [SerializeField] CollectablesIcons collectablesIcons;
    [SerializeField] GameObject panel;
    [SerializeField] string levelName;

    private void Awake()
    {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }

    void Start()
    {
        LevelData level = GameManager.Instance.GetLevelData(levelName);

        if (level != null)
        {
            collectablesIcons.RefreshFromLevelData(level);
            panel.SetActive(true);
        }
    }
}