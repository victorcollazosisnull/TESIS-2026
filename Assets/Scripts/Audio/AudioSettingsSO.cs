using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Audio Settings")]
public class AudioSettingsSO : ScriptableObject
{
    [SerializeField] private AudioMixer mixer;

    private const string MASTER = "Master";
    private const string MUSIC = "Music";
    private const string SFX = "SFX";

    public void Load()
    {
        Set(MASTER, PlayerPrefs.GetFloat(MASTER, 0.8f));
        Set(MUSIC, PlayerPrefs.GetFloat(MUSIC, 0.8f));
        Set(SFX, PlayerPrefs.GetFloat(SFX, 0.8f));
    }

    public void SetMaster(float v) => Set(MASTER, v);
    public void SetMusic(float v) => Set(MUSIC, v);
    public void SetSFX(float v) => Set(SFX, v);

    private void Set(string param, float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);

        float dB = Mathf.Log10(value) * 20f;
        mixer.SetFloat(param, dB);

        PlayerPrefs.SetFloat(param, value);
    }

    public float GetMaster() => PlayerPrefs.GetFloat(MASTER, 0.8f);
    public float GetMusic() => PlayerPrefs.GetFloat(MUSIC, 0.8f);
    public float GetSFX() => PlayerPrefs.GetFloat(SFX, 0.8f);
}