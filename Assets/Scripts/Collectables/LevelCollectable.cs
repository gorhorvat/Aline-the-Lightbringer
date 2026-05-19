using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class LevelCollectable : BaseCollectable
{
    [SerializeField] CollectableType collectableType;

    void Start()
    {
        if (GameManager.Instance.IsLevelCollectableCollected(collectableType, SceneManager.GetActiveScene().name))
            Destroy(gameObject);
    }

    protected override void OnCollected()
    {
        base.OnCollected();
        GameManager.Instance.CollectLevelCollectable(collectableType, SceneManager.GetActiveScene().name);
    }
}