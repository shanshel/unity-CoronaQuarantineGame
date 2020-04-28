using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static EnumsData;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager _inst { get { return _instance; } }

    public Sound[] sounds;
  
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            foreach (var s in this.sounds)
            {
                s.audioSource = gameObject.AddComponent<AudioSource>();
                s.audioSource.clip = s.clip;
                s.audioSource.volume = s.volume;
                s.audioSource.pitch = s.pitch;
                s.audioSource.loop = s.isLooping;
                s.audioSource.playOnAwake = s.playOnAwake;
            }
        }
    }

    public void playSoundOnce(EnumsData.SoundEnum soundEnum)
    {
        foreach (var s in this.sounds)
        {
            if (s.sound == soundEnum)
            {
                s.audioSource.Play();
                return;
            }
        }
    }

    public void playMusic(EnumsData.SoundEnum soundEnum)
    {
        foreach (var s in this.sounds)
        {
            if (s.sound == soundEnum && s.isMusic)
            {
                s.audioSource.Play();
                return;
            }
            else if (s.isMusic)
            {
                s.audioSource.Stop();
            }
        }
    }

    public void stopAllMusic()
    {
        foreach (var s in this.sounds)
        {
            if (s.isMusic)
            {
                s.audioSource.Stop();
            }
        }
    }
}
