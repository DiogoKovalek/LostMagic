using UnityEngine;

public class PlayWhenDestoy : MonoBehaviour {

    public AudioClip sound;

    private bool isQuitting = false;
    void OnDestroy() {
        if (isQuitting) return;
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }
    
    void OnApplicationQuit() {
        isQuitting = true;
    }
}
