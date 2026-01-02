using UnityEngine;

public class PlayWhenStart : MonoBehaviour
{
    public AudioClip sound;
    private AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(sound);

        if (audioSource == null) Debug.Log("Sem AudioSource");
        if (sound == null) Debug.Log("Sem Som");
    }
}
