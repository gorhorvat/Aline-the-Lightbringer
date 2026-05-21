using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerGroup sfxMixerGroup;

    public AudioSettings audioSettings;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        ApplySettings();
    }

    float ToDecibels(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
    }

    public void ApplySettings()
    {
        SetMasterVolume(audioSettings.masterVolume);
        SetMusicVolume(audioSettings.musicVolume);
        SetSfxVolume(audioSettings.sfxVolume);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", ToDecibels(value));
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", ToDecibels(value));

    }

    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat("SfxVolume", ToDecibels(value));
    }

    public void PlaySfx(AudioClip clip, Vector3 position, float volume = 1f)
    {
        GameObject obj = new("TempSFX");

        obj.transform.position = position;

        AudioSource source = obj.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.outputAudioMixerGroup = sfxMixerGroup;
        source.Play();

        Destroy(obj, clip.length);
    }
}