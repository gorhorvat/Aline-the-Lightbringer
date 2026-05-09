using UnityEngine;
using UnityEngine.SceneManagement;

public class BeaconCrystal : BaseCollectable
{
    protected override void OnCollected()
    {
        GameManager.Instance.CollectBeaconCrystal(SceneManager.GetActiveScene().name);
    }
}