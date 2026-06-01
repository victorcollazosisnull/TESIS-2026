using UnityEngine;
public class SceneMusicPlayer : MonoBehaviour
{
    [SerializeField] private SoundData sceneMusic;

    private void Start()
    {
        if (sceneMusic == null) return;

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(sceneMusic);
    }
}
