using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;
    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Sounds")]
    [SerializeField] private Sound[] sounds;

    [Header("Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float musicVolume = 0.7f;
    [SerializeField] private float sfxVolume = 1f;

    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSounds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSounds()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary.Add(sound.name, sound);
            }
        }
    }

    public void PlaySound(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            sound.source.volume = sound.volume * sfxVolume * masterVolume;
            sound.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found in AudioManager!");
        }
    }

    public void PlaySoundOneShot(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            sfxSource.PlayOneShot(sound.clip, sound.volume * sfxVolume * masterVolume);
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found in AudioManager!");
        }
    }

    public void PlayMusic(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            musicSource.clip = sound.clip;
            musicSource.volume = sound.volume * musicVolume * masterVolume;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music '{name}' not found in AudioManager!");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSound(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            sound.source.Stop();
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume * masterVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    private void UpdateAllVolumes()
    {
        musicSource.volume = musicVolume * masterVolume;
        foreach (var sound in soundDictionary.Values)
        {
            sound.source.volume = sound.volume * sfxVolume * masterVolume;
        }
    }
}
