using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Game/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    public void Reset()
    {
        masterVolume = 1f;
        musicVolume = 1f;
        sfxVolume = 1f;
    }
}