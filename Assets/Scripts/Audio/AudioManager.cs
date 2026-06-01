using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(SoundData sound)
    {
        if (sound == null || sound.clip == null) return;

        if (sound.type == SoundType.Music)
            PlayMusic(sound);
        else
            PlaySFX(sound);
    }

    private void PlayMusic(SoundData music)
    {
        musicSource.Stop();
        musicSource.clip = music.clip;
        musicSource.loop = music.loop;
        musicSource.volume = music.volume;
        musicSource.Play();
    }

    private void PlaySFX(SoundData sfx)
    {
        sfxSource.PlayOneShot(sfx.clip, sfx.volume);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
