using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] private AudioSettingsSO audioSettings;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        audioSettings.Load();

        masterSlider.value = audioSettings.GetMaster();
        musicSlider.value = audioSettings.GetMusic();
        sfxSlider.value = audioSettings.GetSFX();

        masterSlider.onValueChanged.AddListener(audioSettings.SetMaster);
        musicSlider.onValueChanged.AddListener(audioSettings.SetMusic);
        sfxSlider.onValueChanged.AddListener(audioSettings.SetSFX);
    }
}