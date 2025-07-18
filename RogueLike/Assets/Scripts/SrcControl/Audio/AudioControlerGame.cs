using UnityEngine;

public class AudioControlerGame : MonoBehaviour{
    public AudioClip[] musics;
    private AudioSource audioSource;
    public bool mute;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play() {
        if (!mute) {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            audioSource.clip = sortMusic();
            audioSource.Play();  
        }
    }

    public void Stop() {
        audioSource?.Stop();
    }

    private AudioClip sortMusic() {
        return musics[Random.Range(0, musics.Length)];
    }
}
