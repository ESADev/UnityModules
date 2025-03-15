using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [System.Serializable]
    public class SFX
    {
        public string name; // For easy identification
        [Range(0f, 1f)][DefaultValue(1f)] public float volume = 1f;
        [Range(0.1f, 3f)][DefaultValue(1f)] public float pitch = 1f;
        [Range(0.0f, 1.5f)] public float pitchVariance = 0.1f;
        public AudioClip[] clips; // Variations for natural playback
    }

    public SFXSource sfxSource;
    public List<SFX> sfxList = new List<SFX>();
    private Dictionary<string, SFX> sfxDictionary;

    // Simple object pool for SFXSource objects
    private List<SFXSource> pool = new List<SFXSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxDictionary = new Dictionary<string, SFX>();
        foreach (var sfx in sfxList)
        {
            if (sfxDictionary.ContainsKey(sfx.name))
            {
                Debug.LogWarning("Duplicate SFX name detected: " + sfx.name + ". Only the last one will be used.");
            }
            sfxDictionary[sfx.name] = sfx;
        }

        if (sfxSource == null)
        {
            Debug.LogError("AudioSource prefab is not assigned in SFXManager!");
        }
    }

    // Get an available SFXSource from the pool or instantiate a new one if needed
    private SFXSource GetSFXSource()
    {
        foreach (var source in pool)
        {
            if (!source.audioSource.isPlaying)
            {
                return source;
            }
        }
        SFXSource newSource = Instantiate(sfxSource, transform.position, Quaternion.identity, transform);
        DontDestroyOnLoad(newSource.gameObject);
        pool.Add(newSource);
        return newSource;
    }

    public void PlaySFX(string name)
    {
        if (!sfxDictionary.ContainsKey(name))
        {
            Debug.LogError("SFX '" + name + "' not found!");
            return;
        }

        var sfx = sfxDictionary[name];
        if (sfx.clips.Length == 0)
        {
            Debug.LogWarning("No clips assigned to SFX '" + name + "'!");
            return;
        }

        SFXSource audioObject = GetSFXSource();
        AudioSource audioSource = audioObject.audioSource;
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on the SFXSource!");
            return;
        }

        float pitch = sfx.pitch + Random.Range(-sfx.pitchVariance, sfx.pitchVariance);
        audioSource.clip = sfx.clips[Random.Range(0, sfx.clips.Length)];
        audioSource.pitch = pitch;
        audioSource.volume = sfx.volume;
        audioSource.Play();

        // Start a coroutine to "release" the AudioSource back to the pool after playback
        StartCoroutine(ReturnToPoolAfterPlaying(audioObject, audioSource.clip.length / pitch));
    }

    private IEnumerator ReturnToPoolAfterPlaying(SFXSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.audioSource.clip = null;
    }
}
