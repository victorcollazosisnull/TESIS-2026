using UnityEngine;

public enum SoundType { Music, SFX }

[CreateAssetMenu(fileName = "Sound", menuName = "Scriptable Objects/Audio/Sound", order = 2)]
public class SoundData : ScriptableObject
{
    public SoundType type;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    public bool loop;
}
