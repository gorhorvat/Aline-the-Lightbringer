using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsPanel;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    AudioSettings audioSettings;

    public void Show(AudioSettings settings)
    {
        audioSettings = settings;
        masterSlider.value = audioSettings.masterVolume;
        musicSlider.value = audioSettings.musicVolume;
        sfxSlider.value = audioSettings.sfxVolume;
        optionsPanel.SetActive(true);
    }

    public void Hide() => optionsPanel.SetActive(false);

    public void OnMasterVolumeChanged(float value)
    {
        audioSettings.masterVolume = value;
        AudioManager.Instance.SetMasterVolume(value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        audioSettings.musicVolume = value;
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnSfxVolumeChanged(float value)
    {
        audioSettings.sfxVolume = value;
        AudioManager.Instance.SetSfxVolume(value);
    }
}