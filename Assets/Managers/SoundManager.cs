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

    public int maxInitPerSound;
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
                initSound(s);
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

    public void stopSound(EnumsData.SoundEnum soundEnum)
    {
        foreach (var s in this.sounds)
        {
            if (s.sound == soundEnum)
            {
                s.audioSource.Stop();
                return;
            }
        }
    }
    public void playSoundOnceAt(EnumsData.SoundEnum soundEnum, Vector3 playPosition)
    {
        playPosition.z = 0f;
        foreach (var s in this.sounds)
        {
            if (s.sound == soundEnum)
            {
                if (s.audioSource.isPlaying)
                {
                    AudioSource.PlayClipAtPoint(s.clip, playPosition);
                }
                else
                {
                    s.audioSource.transform.position = playPosition;
                    s.audioSource.Play();
                }
                
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

    public void initSound(Sound s)
    {
        var gmObj = new GameObject();
        gmObj.transform.SetParent(transform);
        gmObj.transform.position = Vector3.zero;
        s.audioSource = gmObj.AddComponent<AudioSource>();
        s.audioSource.clip = s.clip;
        s.audioSource.volume = s.volume;
        s.audioSource.pitch = s.pitch;
        s.audioSource.loop = s.isLooping;
        s.audioSource.playOnAwake = s.playOnAwake;
        s.audioSource.minDistance = 10f;
        s.audioSource.maxDistance = s.maxDistanceToHear;
        s.audioSource.spatialBlend = (s.is3D) ? 1 : 0;
    }

  
}
