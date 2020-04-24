using UnityEngine;
using UnityEngine.Audio;
using static EnumsData;

[System.Serializable]
public class Sound
{
    public SoundEnum sound;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool playOnAwake = false;
    public bool isLooping = false;

    public AudioClip clip;

    public bool isMusic = false;
    [HideInInspector]
    public AudioSource audioSource;

}
