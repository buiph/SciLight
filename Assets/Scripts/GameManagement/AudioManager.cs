using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // This is a singleton object
    [SerializeField] internal SettingsData settingsData;
    public AudioSource themeSongAudioSource;

    public Sound[] themeSounds;
    internal Sound currTheme;
    public Sound[] sounds;
    private static Dictionary<string, float> soundTimerDictionary;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        soundTimerDictionary = new Dictionary<string, float>();

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.isLoop;

            if (sound.hasCooldown)
            {
                soundTimerDictionary[sound.name] = sound.cooldown;
            }
        }
    }

    private void Start()
    {
        Assert.IsNotNull( settingsData );
        Assert.IsNotNull( themeSongAudioSource );

        SettingsManager.Instance.OnMusicVolumeChange += UpdateMusicVolume;
    }

    void OnDestroy()
    {
        SettingsManager.Instance.OnMusicVolumeChange -= UpdateMusicVolume;
    }

    /// <summary>
    /// Play a sound given its string name
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }

        if (!CanPlaySound(sound))return;
        sound.source.volume = (settingsData.sfxVolume / 100) * (sound.volume);
        sound.source.Play();
    }

    /// <summary>
    /// Play a sound given its string name
    /// </summary>
    /// <param name="name"></param>
    public void PlayTheme(string name)
    {
        currTheme = Array.Find(themeSounds, s => s.name == name);

        if (themeSongAudioSource.isPlaying)
        {
            themeSongAudioSource.Stop();
        }

        themeSongAudioSource.clip = currTheme.clip;

        if (currTheme == null)
        {
            Debug.LogError("Theme " + name + " Not Found!");
            return;
        }

        if (!CanPlaySound(currTheme))return;
        themeSongAudioSource.volume = (settingsData.musicVolume / 100) * (currTheme.volume);
        themeSongAudioSource.Play();
    }

    /// <summary>
    /// Stop playing a sound given its string name
    /// </summary>
    /// <param name="name"></param>
    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }

        sound.source.Stop();
    }

    /// <summary>
    /// Updates the theme music volume
    /// </summary>
    public void UpdateMusicVolume()
    {
        themeSongAudioSource.volume = (settingsData.musicVolume / 100) * (currTheme.volume);
    }

    /// <summary>
    /// Check whether a sound is still in it's cooldown period
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    private static bool CanPlaySound(Sound sound)
    {
        if (soundTimerDictionary.ContainsKey(sound.name))
        {
            float lastTimePlayed = soundTimerDictionary[sound.name];

            if (lastTimePlayed + sound.clip.length < Time.time)
            {
                soundTimerDictionary[sound.name] = Time.time;
                return true;
            }

            return false;
        }

        return true;
    }
}
