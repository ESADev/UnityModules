using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXSource : MonoBehaviour
{
    public AudioSource audioSource;

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No AudioSource found on " + gameObject.name);
            }
        }
    }
}